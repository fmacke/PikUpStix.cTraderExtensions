using Domain.Entities;

namespace Application.Business.BackTest.Reports
{
    public class AnnualReturn
    {
        public int TestId { get; set; }
        public decimal ReturnInCash { get; set; }
        public decimal ReturnAsPercentofInvestmentCapital { get; set; }
        public int Year { get; set; }
    }
    public class AnnualReturns : Dictionary<int, AnnualReturn>
    {
        public AnnualReturns(int testId, IEnumerable<TestTrade> trades)
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
                var initialMargin = CalculateMarginToDate(resultsToProcess, year);
                var annualMargin = CalculateMarginThisYearOnly(resultsToProcess, year);
                this.Add(year, new AnnualReturn()
                {
                    ReturnAsPercentofInvestmentCapital = annualMargin / initialMargin,
                    ReturnInCash = annualMargin,
                    TestId = testId
                });
            }
        }

        private decimal CalculateMarginToDate(IOrderedEnumerable<TestTrade> trades, int upToButExludingYear)
        {
            var filteredTrades = trades.Where(x => Convert.ToDateTime(x.ClosedAt).Year < upToButExludingYear); 
            if (filteredTrades.Any()) 
                return filteredTrades.Sum(x => x.Margin); 
            return 0;
        }
        private decimal CalculateMarginThisYearOnly(IOrderedEnumerable<TestTrade> trades, int thisYear)
        {
            var filteredTrades = trades.Where(x => Convert.ToDateTime(x.ClosedAt).Year == thisYear);
            if (filteredTrades.Any())
                return filteredTrades.Sum(x => x.Margin);
            return 0;
        }
    }
}