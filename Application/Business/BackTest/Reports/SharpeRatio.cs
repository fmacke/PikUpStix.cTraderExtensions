using Domain.Entities;

namespace Application.Business.BackTest.Reports
{
    public class SharpeRatio
    {
        public SharpeRatio(IReadOnlyCollection<TestTrade> results)
        {
            AveragePnL = results.Average(x => x.Margin);
            var dailyPnL = new List<double>();

            foreach (TestTrade result in results)
            {
                dailyPnL.Add(Convert.ToDouble(result.Margin));
            }
            if (results.Count > 0)
            {
                var stdDevOfAverageReturns = new StandardDeviation(dailyPnL);
                StandardDeviationOfPnL = stdDevOfAverageReturns.Calculate;
                if (StandardDeviationOfPnL == 0)
                    Value = 0;
                else
                    Value = Math.Round(AveragePnL / StandardDeviationOfPnL, 4);
            }
        }

        public double AveragePnL { get; private set; }
        public double StandardDeviationOfPnL { get; private set; }
        public double Value { get; private set; }
    }
}