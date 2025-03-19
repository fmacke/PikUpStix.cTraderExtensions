using Domain.Entities;

namespace Application.Business.BackTest.Reports
{
    public class AnnualReturn
    {
        public int TestId { get; set; }
        public double ReturnInCash { get; set; }
        public double ReturnAsPercentofInvestmentCapital { get; set; }
        public int Year { get; set; }
    }
    public class AnnualReturns : Dictionary<int, AnnualReturn>
    {
        public AnnualReturns(int testId, IEnumerable<TestTrade> trades, double initialCapitalInvestment)
        {
            var resultsToProcess = trades.OrderBy(x => x.ClosedAt);
            //get distinct list of years from results
            var years = resultsToProcess.Select(x => new { Convert.ToDateTime(x.ClosedAt).Year })
                .Distinct()
                .ToList()
                .Select(p => p.Year);

            //foreach year in years, calculate annual return
            foreach (var year in years)
            {
                var initialCapital = CalculateCapitalAtStartOfYear(resultsToProcess, year, initialCapitalInvestment);            
                var marginThisYear = CalculateMarginThisYearOnly(resultsToProcess, year);

                this.Add(year, new AnnualReturn()
                {
                    ReturnAsPercentofInvestmentCapital = initialCapital == 0 ? (marginThisYear / 100) : (marginThisYear / initialCapital),
                    ReturnInCash = marginThisYear,
                    TestId = testId
                });
            }
        }

        private double CalculateCapitalAtStartOfYear(IOrderedEnumerable<TestTrade> trades, int upToButExcludingYear, double initialCapital)
        {
            var filteredTrades = trades.Where(x => Convert.ToDateTime(x.ClosedAt).Year < upToButExcludingYear); 
            if (filteredTrades.Any())
                initialCapital += filteredTrades.Sum(x => x.Margin); 
            return initialCapital;
        }
        private double CalculateMarginThisYearOnly(IOrderedEnumerable<TestTrade> trades, int thisYear)
        {
            var filteredTrades = trades.Where(x => Convert.ToDateTime(x.ClosedAt).Year == thisYear);
            if (filteredTrades.Any())
                return filteredTrades.Sum(x => x.Margin);
            return 0;
        }
    }
}