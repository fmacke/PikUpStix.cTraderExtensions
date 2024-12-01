using Domain.Entities;

namespace Application.Business.BackTest.Reports
{
    public class AnnualReturns : List<Test_AnnualReturns>
    {
        public AnnualReturns(int testId, IEnumerable<Test_Trades> results)
        {
            //var resultsToProcess = results.OrderBy(x => x.ClosedAt);
            ////get distinct list of years from results
            //var years = resultsToProcess.Select(x => new { Convert.ToDateTime(x.ClosedAt).Year })
            //    .Distinct()
            //    .ToList()
            //    .Select(p => p.Year);
            ////foreach year in years, calculate annual return
            //foreach (var year in years)
            //{
            //    var initialMargin = Convert.ToDecimal(resultsToProcess.First(x => Convert.ToDateTime(x.ClosedAt).Year == year).CumulativeMargin);
            //    var annualMargin = resultsToProcess.Where(x => Convert.ToDateTime(x.CurrentDate).Year == year).Sum(result => Convert.ToDecimal(result.Margin));
            //    this.Add(new Test_AnnualReturns()
            //    {
            //        ReturnAsPercentofInvestmentCapital = annualMargin / initialMargin,
            //        ReturnInCash = annualMargin,
            //        Year = new DateTime(year, 1,1),
            //        TestId = testId
            //    });
            //}
        }
    }
}