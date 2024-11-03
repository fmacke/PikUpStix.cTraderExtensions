using Domain.Entities;

namespace Application.Volatility
{
    /// <summary>
    /// Calculates the price volatility of a financial instrument over a specified period.
    /// Returns the standard deviation of the percentage change in price over the given number of periods.
    /// </summary>
    public class PriceVolatility
    {
        public PriceVolatility(List<HistoricalData> prices, int numberOfPeriods)
        {
            NumberOfPeriods = numberOfPeriods;
            if (prices.Any() && HasValidData(prices))
            {
                if (numberOfPeriods >= prices.Count)
                    NumberOfPeriods = prices.Count - 1;

                double[] pricePoints = GetPrices(prices);
                double[] pricePercentageDifferences = GetPercentageDailyPriceChange(pricePoints);
                StandardDeviation = CalculateStdDev(pricePercentageDifferences);
            }
            else
                StandardDeviation = 0;
        }

        private bool HasValidData(List<HistoricalData> prices)
        {
            bool validData = false;
            foreach (var historicalData in prices)
                if (historicalData.ClosePrice.HasValue)
                    validData = true;
            return validData;
        }

        public double StandardDeviation { get; private set; }
        public int NumberOfPeriods { get; private set; }

        private double[] GetPrices(List<HistoricalData> prices)
        {
            var pricePoints = new double[NumberOfPeriods];
            int count = 0;

            var mostRecentPrices = new List<HistoricalData>();

            foreach (HistoricalData historicalPrice in prices.OrderByDescending(x => x.Date))
            {
                if (count < NumberOfPeriods)
                {
                    if (historicalPrice.ClosePrice != 0)
                    {
                        mostRecentPrices.Add(historicalPrice);
                    }
                    else
                    {
                       // new ErrorHandler("PriceVolatility", "GetPrices",
                            new Exception("PriceVolatility.GetPricesHistorical close price should not be zero");
                    }
                    count++;
                }
                else
                {
                    break;
                }
            }
            count = 0;
            foreach (HistoricalData historicalPrice in mostRecentPrices.OrderBy(x => x.Date))
            {
                if (count < NumberOfPeriods)
                {
                    pricePoints[count] = Convert.ToDouble(historicalPrice.ClosePrice);
                    count++;
                }
                else
                {
                    break;
                }
            }
            return pricePoints;
        }

        private double[] GetPercentageDailyPriceChange(double[] prices)
        {
            foreach (double price in prices)
            {
                if (price == 0)
                    throw new Exception("Price cannot be zero (0)");
            }
            var pricePercentageDifferences = new double[prices.Count() - 1];
            int priceDifferenceCounter = 0;

            for (int i = 1; i < prices.Count(); i++)
            {
                double priceDiff = prices[i] - prices[i - 1];
                double percentageDiff = priceDiff / prices[i - 1];
                pricePercentageDifferences[priceDifferenceCounter] = percentageDiff;
                priceDifferenceCounter++;
            }
            return pricePercentageDifferences;
        }

        public double CalculateStdDev(IEnumerable<double> values)
        {
            var stdDev = new StandardDeviation(values);
            return stdDev.Calculate;
        }
    }
}