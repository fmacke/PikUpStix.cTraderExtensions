using Domain.Entities;

namespace Application.Business.Calculations
{
    public class SharpeRatio : ICalculate
    {
        private IReadOnlyCollection<Position> Results { get; set; }
        private double AveragePnL { get;  set; } = 0;
        private double StandardDeviationOfPnL { get;  set; }
        private double Value { get; set; } = 0;

        public SharpeRatio(IReadOnlyCollection<Position> results)
        {
            Results = results ?? throw new ArgumentNullException(nameof(results));            
        }
        private void CalculateStdDeviationOfPnL()
        {
            var dailyPnL = Results.Select(result => Convert.ToDouble(result.Margin)).ToList();
            StandardDeviationOfPnL = new StandardDeviation(dailyPnL.ToArray()).Calculate();
        }
        public double Calculate()
        {
            if (Results.Count > 0)
            {
                AveragePnL = Results.Average(x => x.Margin);
                CalculateStdDeviationOfPnL();
                if (StandardDeviationOfPnL != 0)
                {
                    Value = Math.Round(AveragePnL / StandardDeviationOfPnL, 4);
                }
            }
            return Value;
        }
    }
}