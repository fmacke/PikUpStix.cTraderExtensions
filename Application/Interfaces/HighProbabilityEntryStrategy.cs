using Application.Business.Market;
using Application.Business.Positioning.Instructions;
using Application.Business.Positioning.Validation;
using Domain.Entities;

namespace Application.Interfaces
{
    public class HighProbabilityEntryStrategy 
    {
        // Strategy parameters (can be adjusted)
        public readonly int _fastEmaPeriod;
        public readonly int _slowEmaPeriod;
        public readonly int _rsiPeriod;
        public readonly decimal _rsiOversoldThreshold;
        public readonly decimal _rsiOverboughtThreshold;

        // Weights for signal strength
        private const decimal PriceAboveSlowEmaWeight = 0.3m;
        private const decimal FastEmaAboveSlowEmaWeight = 0.3m;
        private const decimal RsiCrossUpWeight = 0.4m; // Gives RSI cross a higher weight for entry timing

        private const decimal PriceBelowSlowEmaWeight = 0.3m;
        private const decimal FastEmaBelowSlowEmaWeight = 0.3m;
        private const decimal RsiCrossDownWeight = 0.4m; // Gives RSI cross a higher weight for entry timing

        public List<string> LogMessages { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<Test_Parameter> TestParameters { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IValidationService GetValidationService()
        {
            throw new NotImplementedException();
        }

        public HighProbabilityEntryStrategy(
            int fastEmaPeriod = 50,
            int slowEmaPeriod = 200,
            int rsiPeriod = 14,
            decimal rsiOversoldThreshold = 30,
            decimal rsiOverboughtThreshold = 70)
        {
            _fastEmaPeriod = fastEmaPeriod;
            _slowEmaPeriod = slowEmaPeriod;
            _rsiPeriod = rsiPeriod;
            _rsiOversoldThreshold = rsiOversoldThreshold;
            _rsiOverboughtThreshold = rsiOverboughtThreshold;
        }

        /// <summary>
        /// Calculates the Exponential Moving Average (EMA) for a given set of prices.
        /// </summary>
        /// <param name="prices">List of closing prices.</param>
        /// <param name="period">The period for the EMA calculation.</param>
        /// <returns>A list of EMA values, or an empty list if not enough data.</returns>
        private List<decimal> CalculateEMA(List<decimal> prices, int period)
        {
            if (prices == null || prices.Count < period)
            {
                return new List<decimal>();
            }

            var emas = new List<decimal>();
            decimal multiplier = 2m / (period + 1);

            // Calculate initial SMA for the first 'period' values
            decimal initialSma = prices.Take(period).Average();
            emas.Add(initialSma);

            // Calculate subsequent EMAs
            for (int i = period; i < prices.Count; i++)
            {
                decimal ema = (prices[i] - emas.Last()) * multiplier + emas.Last();
                emas.Add(ema);
            }
            return emas;
        }

        /// <summary>
        /// Calculates the Relative Strength Index (RSI) for a given set of prices.
        /// </summary>
        /// <param name="prices">List of closing prices.</param>
        /// <param name="period">The period for the RSI calculation.</param>
        /// <returns>A list of RSI values, or an empty list if not enough data.</returns>
        private List<decimal> CalculateRSI(List<decimal> prices, int period)
        {
            if (prices == null || prices.Count <= period)
            {
                return new List<decimal>();
            }

            List<decimal> rsiValues = new List<decimal>();
            List<decimal> gains = new List<decimal>();
            List<decimal> losses = new List<decimal>();

            for (int i = 1; i < prices.Count; i++)
            {
                decimal change = prices[i] - prices[i - 1];
                gains.Add(Math.Max(0, change));
                losses.Add(Math.Max(0, -change));
            }

            // Calculate initial average gain and loss
            decimal avgGain = gains.Take(period).Average();
            decimal avgLoss = losses.Take(period).Average();

            decimal rs = (avgLoss == 0) ? 100 : (avgGain / avgLoss); // Handle division by zero
            rsiValues.Add(100 - (100 / (1 + rs)));

            // Calculate subsequent RS and RSI values
            for (int i = period; i < gains.Count; i++)
            {
                avgGain = (avgGain * (period - 1) + gains[i]) / period;
                avgLoss = (avgLoss * (period - 1) + losses[i]) / period;

                rs = (avgLoss == 0) ? 100 : (avgGain / avgLoss); // Handle division by zero
                rsiValues.Add(100 - (100 / (1 + rs)));
            }

            return rsiValues;
        }

        /// <summary>
        /// Generates an analogue trade signal based on the current and historical price data,
        /// ranging from -1 (strong sell) to +1 (strong buy).
        /// </summary>
        /// <param name="historicalBars">List of historical bars, ordered from oldest to newest.</param>
        /// <returns>A decimal value between -1 and +1 representing the signal strength.</returns>
        public decimal GenerateAnalogueSignal(List<Bar> historicalBars)
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

        public List<IPositionInstruction> CalculateChanges(List<IMarketInfo> marketInfos)
        {
            throw new NotImplementedException();
        }

        public void LoadDefaultParameters(Dictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
