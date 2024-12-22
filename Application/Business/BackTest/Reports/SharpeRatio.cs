using Domain.Entities;

namespace Application.Business.BackTest.Reports
{
    public class SharpeRatio
    {
        public SharpeRatio(IReadOnlyCollection<TestTrade> results)
        {
            AveragePnL = Convert.ToDecimal(results.Average(x => x.Margin));
            var dailyPnL = new List<double>();

            foreach (TestTrade result in results)
            {
                dailyPnL.Add(Convert.ToDouble(result.Margin));
            }
            if (results.Count > 0)
            {
                var stdDevOfAverageReturns = new StandardDeviation(dailyPnL);
                StandardDeviationOfPnL = Convert.ToDecimal(stdDevOfAverageReturns.Calculate);
                if (StandardDeviationOfPnL == 0)
                    Get = 0;
                else
                    Get = Math.Round(AveragePnL / StandardDeviationOfPnL, 4);
            }
        }

        public decimal AveragePnL { get; private set; }
        public decimal StandardDeviationOfPnL { get; private set; }
        public decimal Get { get; private set; }
    }
}