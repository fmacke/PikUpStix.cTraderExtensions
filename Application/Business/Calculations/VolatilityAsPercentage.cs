using Domain.Entities;

namespace Application.Business.Calculations
{
    /// <summary>
    /// Calculates the volatility of a given list of datapoint values over a specified period.
    /// Returns the standard deviation of the percentage change in the datapoint values over the given number of periods.
    /// </summary>
    using System;
    using System.Linq;

    public class VolatilityAsPercentage : ICalculate
    {
        private int NumberOfPeriods { get; set; }
        private double[] DataPoints { get; set; }

        public VolatilityAsPercentage(double[] dataPoints, int numberOfPeriods)
        {
            DataPoints = dataPoints ?? throw new ArgumentNullException(nameof(dataPoints));
            NumberOfPeriods = numberOfPeriods >= dataPoints.Length ? dataPoints.Length : numberOfPeriods;
        }

        public double Calculate()
        {
            if (DataPoints.Any())
            {
                double[] lastEntries = GetLastEntries(NumberOfPeriods);
                double[] pricePercentageDifferences = GetPercentageChangePerPeriod(lastEntries);
                return new StandardDeviation(pricePercentageDifferences).Calculate();
            }
            return 0;
        }

        private double[] GetLastEntries(int numberOfPeriods)
        {
            if (numberOfPeriods < 0)
                throw new ArgumentOutOfRangeException(nameof(numberOfPeriods), "Number of entries must be non-negative.");
            if (numberOfPeriods > DataPoints.Length)
                throw new ArgumentException("Number of entries exceeds array length.");

            double[] result = new double[numberOfPeriods];
            Array.Copy(DataPoints, DataPoints.Length - numberOfPeriods, result, 0, numberOfPeriods);
            return result;
        }

        private static double[] GetPercentageChangePerPeriod(double[] prices)
        {
            if (prices.Any(price => price == 0))
                throw new Exception("Price cannot be zero (0)");
            return prices
                .Skip(1)
                .Select((price, index) => (price - prices[index]) / prices[index] * 100)
                .ToArray();
        }
    }
}