using Application.Business.Calculations;
using Application.Business.Market;
using Application.Business.Positioning.Instructions;
using Application.Business.Positioning.Validation;
using Application.Business.Positioning;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using System.Diagnostics;
using Application.Business.Indicator;

namespace Robots.Strategies
{
    public class HighProbabilityEntryStrategy : IStrategy
    {
        public List<string> LogMessages { get; set; } = new List<string>();
        public List<IPositionInstruction> PositionInstructions { get; private set; } = new List<IPositionInstruction>();
        public IValidationService ValidationService { get; set; } = new ValidationService();
        public List<Test_Parameter> TestParameters { get; set; }

        private  int _fastEmaPeriod;
        private  int _slowEmaPeriod;
        private  int _rsiPeriod;
        private  decimal _rsiOversoldThreshold;
        private  decimal _rsiOverboughtThreshold;

        public List<IPositionInstruction> CalculateChanges(List<IMarketInfo> marketInfos)
        {
            foreach (var marketInfo in marketInfos)
            {
                if (marketInfo?.Bars == null || marketInfo.Bars.Count == 0)
                {
                    LogMessages.Add($"No historical data available for {marketInfo.SymbolName}");
                    continue;
                }
                var signal = GenerateAnalogueSignal(marketInfo.Bars);
                if (signal != TradeSignal.Hold)
                {
                    PositionInstructions.Add(new OpenInstruction(
                        PositionCreator.CreatePosition(signal, marketInfo.Ask, _fastEmaPeriod, _slowEmaPeriod, _rsiPeriod, marketInfo, ValidationService),
                        ValidationService));
                }
            }
            
            
            return PositionInstructions;
        }
        public decimal GenerateAnalogueSignal(List<HistoricalData> historicalBars)
        {
            if (historicalBars == null || historicalBars.Count < Math.Max(_slowEmaPeriod, _rsiPeriod + 1))
            {
                // Not enough data to calculate indicators
                // Console.WriteLine("Not enough historical data to generate a signal."); // Commented out for cleaner output
                return 0.0m; // Neutral signal
            }

            // Extract closing prices
            List<decimal> closePrices = historicalBars.Select(b => b.Close).ToList();

            // Calculate EMAs
            List<decimal> fastEmas = CalculateEMA(closePrices, _fastEmaPeriod);
            List<decimal> slowEmas = CalculateEMA(closePrices, _slowEmaPeriod);

            // Calculate RSIs
            List<decimal> rsiValues = CalculateRSI(closePrices, _rsiPeriod);

            // Ensure we have enough calculated indicator values for the most recent bar
            if (fastEmas.Count == 0 || slowEmas.Count == 0 || rsiValues.Count == 0)
            {
                // Console.WriteLine("Error calculating indicators. Not enough data for required periods."); // Commented out for cleaner output
                return 0.0m; // Neutral signal
            }

            decimal currentPrice = historicalBars.Last().Close;
            decimal currentFastEma = fastEmas.Last();
            decimal currentSlowEma = slowEmas.Last();
            decimal currentRsi = rsiValues.Last();

            // To check for RSI cross, we need the previous RSI value
            decimal previousRsi = (rsiValues.Count > 1) ? rsiValues[rsiValues.Count - 2] : currentRsi;


            decimal signalStrength = 0.0m; // Initialize signal strength at neutral

            // --- Evaluate Long Conditions ---
            // 1. Price above Slow EMA (Trend confirmation)
            if (currentPrice > currentSlowEma)
            {
                signalStrength += PriceAboveSlowEmaWeight;
            }

            // 2. Fast EMA above Slow EMA (Trend strength/alignment)
            if (currentFastEma > currentSlowEma)
            {
                signalStrength += FastEmaAboveSlowEmaWeight;
            }

            // 3. RSI confirms momentum shifting from oversold to normal (Entry timing)
            if (previousRsi <= _rsiOversoldThreshold && currentRsi > _rsiOversoldThreshold)
            {
                signalStrength += RsiCrossUpWeight;
            }

            // --- Evaluate Short Conditions ---
            // 1. Price below Slow EMA (Trend confirmation)
            if (currentPrice < currentSlowEma)
            {
                signalStrength -= PriceBelowSlowEmaWeight;
            }

            // 2. Fast EMA below Slow EMA (Trend strength/alignment)
            if (currentFastEma < currentSlowEma)
            {
                signalStrength -= FastEmaBelowSlowEmaWeight;
            }

            // 3. RSI confirms momentum shifting from overbought to normal (Entry timing)
            if (previousRsi >= _rsiOverboughtThreshold && currentRsi < _rsiOverboughtThreshold)
            {
                signalStrength -= RsiCrossDownWeight;
            }

            // Normalize the signal strength to be between -1 and +1
            // The maximum possible positive score is PriceAboveSlowEmaWeight + FastEmaAboveSlowEmaWeight + RsiCrossUpWeight
            // The maximum possible negative score is -(PriceBelowSlowEmaWeight + FastEmaBelowSlowEmaWeight + RsiCrossDownWeight)
            // In this case, max positive is 0.3 + 0.3 + 0.4 = 1.0
            // And max negative is -(0.3 + 0.3 + 0.4) = -1.0
            // So, no explicit normalization step is needed if weights sum to 1.0 for each direction.
            // If you add more conditions, you might need a division by the max possible sum of weights.

            Console.WriteLine($"Analogue Signal: {signalStrength:F2} (Price: {currentPrice:F5}, FastEMA: {currentFastEma:F5}, SlowEMA: {currentSlowEma:F5}, RSI: {currentRsi:F2})");
            return signalStrength;
        }
        public void LoadDefaultParameters(Dictionary<string, string> parameters)
        {
            _fastEmaPeriod = 50;
            _slowEmaPeriod = 200;
            _rsiPeriod = 14;
            _rsiOversoldThreshold = 30;
            _rsiOverboughtThreshold = 70;
            TestParameters.Add(new Test_Parameter() { Name = "fastEmaPeriod[int]", Value = _fastEmaPeriod.ToString() });
            TestParameters.Add(new Test_Parameter() { Name = "slowEmaPeriod[int]", Value = _slowEmaPeriod.ToString() });
            TestParameters.Add(new Test_Parameter() { Name = "rsiPeriod[int]", Value = _rsiPeriod.ToString() });
            TestParameters.Add(new Test_Parameter() { Name = "rsiOversoldThreshold[decimal]", Value = _rsiOversoldThreshold.ToString() });
            TestParameters.Add(new Test_Parameter() { Name = "rsiOverboughtThreshold[decimal]", Value = _rsiOverboughtThreshold.ToString() });
        }
    }
    public class VolumePriceAnalysis : IStrategy
    {
        public List<string> LogMessages { get; set; } = new List<string>();
        public List<IPositionInstruction> PositionInstructions { get; private set; } = new List<IPositionInstruction>();
        public IValidationService ValidationService { get; set; } = new ValidationService();
        public List<Test_Parameter> TestParameters { get; set; }
        private Ewmac ewmacIndicator = new Ewmac();
        private double ewmacDecay = 0.05; // Example decay factor
        private double previousEWMA = 0.0;  // Initialize previous EWMA
        private double priceWeight = 0.7; // Weight for price in forecast calculation
        private double volumeWeight = 0.3; // Weight for volume in forecast calculation
        private int periods = 21; // Default periods for EWMA calculation
        private double riskPerTrade = 0.05; // Default risk per trade (5%)
        private double stopLossAmount = 0.02; // Default stop loss amount (2% of entry price)


        public List<IPositionInstruction> CalculateChanges(List<IMarketInfo> marketInfos)
        {
            SetTestParameters();
            PositionInstructions.Clear();

            foreach (var marketInfo in marketInfos)
            {
                if (marketInfo?.Bars == null || marketInfo.Bars.Count < periods)
                {
                    LogMessages.Add($"Insufficient data for {marketInfo?.SymbolName ?? "Unknown Symbol"}");
                    continue;
                }
                var forecast = CalculateForecast(periods, marketInfo);
                //Debug.Print(forecast.ToString("F2"));
                ClosePosition(marketInfo, forecast);
                if (!marketInfo.Positions.Any(p => p.Status == PositionStatus.OPEN))
                {          
                    
                    OpenPosition(marketInfo, marketInfo.Ask, forecast);
                }
            }
            return PositionInstructions;
        }

        public void LoadDefaultParameters(Dictionary<string, string> parameters)
        {
            SetTestParameters();
        }

        private void SetTestParameters()
        {
            foreach (var param in TestParameters)
            {
                if (param.Name == "Periods[Int]")
                {
                    periods = Convert.ToInt32(param.Value);
                }
                else if (param.Name == "PriceWeight[Double]")
                {
                    priceWeight = Convert.ToDouble(param.Value);
                }
                else if (param.Name == "VolumeWeight[Double]")
                {
                    volumeWeight = Convert.ToDouble(param.Value);
                }
                else if (param.Name == "RiskPerTrade[Double]")
                {
                    riskPerTrade = Convert.ToDouble(param.Value);
                }
                else if (param.Name == "StopLossAmount[Double]")
                {
                    stopLossAmount = Convert.ToDouble(param.Value);
                }
            }
        }

        private double CalculateForecast(int dataAmount, IMarketInfo marketInfo)
        {
            // Calculate historical volume stats
            var recentMarketData = marketInfo.Bars.Select(b => b.Volume).TakeLast(dataAmount).ToArray();
            recentMarketData = recentMarketData.Take(recentMarketData.Length - 1).ToArray();  // remove current bar volume
            double meanVolume = recentMarketData.Average();
            double volumeStdDev = new StandardDeviation(recentMarketData).Calculate();

            // Calculate historical price stats
            var averagePrices = marketInfo.Bars.Select(b => (b.OpenPrice + b.HighPrice + b.LowPrice) / 3).TakeLast(dataAmount).ToArray();
            averagePrices = averagePrices.Take(averagePrices.Length - 1).ToArray(); // remove current bar price
            double meanPrice = averagePrices.Average();
            double priceStdDev = new StandardDeviation(averagePrices).Calculate();

            // Current market data
            var volume = marketInfo.LastBar.Volume;

            // **Compute Forecast (-1 to 1)**
            double normalizedPriceChange = (marketInfo.Ask - meanPrice) / priceStdDev;
            double normalizedVolumeStrength = (volume - meanVolume) / volumeStdDev;

            // Combine and weight price vs. volume impact
            return Math.Clamp((priceWeight * normalizedPriceChange) + (volumeWeight * normalizedVolumeStrength), -1, 1);
        }

        private void ClosePosition(IMarketInfo marketInfo, double forecast)
        {
            // Close Positions If Forecast Turns Against Them
            foreach (var position in marketInfo.Positions.Where(p => p.Status == PositionStatus.OPEN))
            {
                if ((position.PositionType == PositionType.BUY && forecast < 0) ||
                    (position.PositionType == PositionType.SELL && forecast > 0))
                {
                    position.Comment = $"Closing position due to forecast reversal (Forecast: {forecast:F2})";
                    PositionInstructions.Add(new CloseInstruction(position, marketInfo.Ask, marketInfo.CursorDate, ValidationService));
                    LogMessages.Add($"Closing {position.PositionType} position for {marketInfo.SymbolName} (Forecast reversed to {forecast:F2})");
                }
            }
        }

        private void OpenPosition(IMarketInfo marketInfo, double price, double forecast)
        {
            // **Compute EWMAC Trend Strength**
            double ewmacTrend = ewmacIndicator.GetEwmac(marketInfo.Ask, ewmacDecay, previousEWMA);

            if (forecast > 0.3 && ewmacTrend > marketInfo.Ask) // Trend confirmation
            {
                Position position = PositionCreator.CreatePosition(PositionType.BUY, forecast, riskPerTrade, null, null, marketInfo, null);
                if (position.Volume > 0)
                {
                    PositionInstructions.Add(new OpenInstruction(position, ValidationService));
                    LogMessages.Add($"Opening Buy position for {marketInfo.SymbolName} (Forecast: {forecast:F2}, EWMAC Confirms Uptrend) at price {price}");
                }
            }
            else if (forecast < -0.3 && ewmacTrend < marketInfo.Ask) // Trend confirmation
            {
                Position position = PositionCreator.CreatePosition(PositionType.SELL, forecast, riskPerTrade, null, null, marketInfo, null);
                if (position.Volume > 0)
                {
                    PositionInstructions.Add(new OpenInstruction(position, ValidationService));
                    LogMessages.Add($"Opening Sell position for {marketInfo.SymbolName} (Forecast: {forecast:F2}, EWMAC Confirms Downtrend) at price {price}");
                }
            }
            else
            {
                LogMessages.Add($"Hold signal for {marketInfo.SymbolName} (Forecast: {forecast:F2}, EWMAC suggests no clear trend) at price {price}");
            }
        }

        public double CalculateAverageElements(double[] values, int numberOfPeriods)
        {
            if (values == null || values.Length == 0 || numberOfPeriods <= 0)
            {
                throw new ArgumentException("Invalid input: array must not be null or empty, and X must be greater than 0.");
            }

            return values.TakeLast(numberOfPeriods).Average();
        }
    }
}