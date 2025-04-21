using Domain.Entities;

namespace Application.Business.Calculations
{
    public class SharpeRatio
    {
        public SharpeRatio(IReadOnlyCollection<Position> results)
        {
            if (results.Count > 0)
            {
                AveragePnL = results.Average(x => x.Margin);
                CalculateStdDeviationOfPnL(results);
                if (StandardDeviationOfPnL != 0)
                {
                    Value = Math.Round(AveragePnL / StandardDeviationOfPnL, 4);
                }
            }
        }

        private void CalculateStdDeviationOfPnL(IReadOnlyCollection<Position> results)
        {
            var dailyPnL = results.Select(result => Convert.ToDouble(result.Margin)).ToList();
            StandardDeviationOfPnL = new StandardDeviation(dailyPnL.ToArray()).Calculate();
        }

        public double AveragePnL { get; private set; } = 0;
        public double StandardDeviationOfPnL { get; private set; }
        public double Value { get; private set; } = 0;
    }

}