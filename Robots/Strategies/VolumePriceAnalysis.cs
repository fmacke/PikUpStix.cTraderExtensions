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
                Debug.Print(forecast.ToString("F2"));
                ClosePosition(marketInfo, forecast);
                if (!marketInfo.Positions.Any(p => p.Status == PositionStatus.OPEN))
                {
                    
                    
                    OpenPosition(marketInfo, marketInfo.Ask, forecast);
                }
            }
            return PositionInstructions;
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