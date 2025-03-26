using Domain.Entities;

namespace Application.Business.Volatility
{
    /// <summary>
    /// Calculates the price volatility of a financial instrument over a specified period.
    /// Returns the standard deviation of the percentage change in price over the given number of periods.
    /// </summary>
    public static class PriceVolatility
    {
        //private double StandardDeviation { get;  }
        //private int NumberOfPeriods { get; private set; }

        public static double OfClosePrices(List<HistoricalData> prices, int numberOfPeriods)
        {
            if (prices.Any() && HasValidData(prices))
            {
                if (numberOfPeriods >= prices.Count)
                    numberOfPeriods = prices.Count - 1;
                double[] pricePoints = GetClosePrices(prices, numberOfPeriods);
                double[] pricePercentageDifferences = GetPercentageDailyPriceChange(pricePoints);
                return StandardDeviation.Calculate(pricePercentageDifferences);
            }
            return 0;
        }

        private static bool HasValidData(List<HistoricalData> prices)
        {
            return prices.Any();           
        }

        private static double[] GetClosePrices(List<HistoricalData> prices, int numberOfPeriods)
        {
            var mostRecentPrices = prices
                .Where(historicalPrice => historicalPrice.ClosePrice != 0)
                .OrderByDescending(x => x.Date)
                .Take(numberOfPeriods)
                .OrderBy(x => x.Date)
                .Select(historicalPrice => Convert.ToDouble(historicalPrice.ClosePrice))
                .ToArray();
            if (mostRecentPrices.Length < numberOfPeriods)
                throw new Exception("PriceVolatility.GetPricesHistorical close price should not be zero");
            return mostRecentPrices;
        }

        private static double[] GetPercentageDailyPriceChange(double[] prices)
        {
            if (prices.Any(price => price == 0))
                throw new Exception("Price cannot be zero (0)");
            return prices
                .Skip(1)
                .Select((price, index) => (price - prices[index]) / prices[index])
                .ToArray();
        }
    }
}