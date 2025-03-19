using Application.Business.BackTest.Reports;
using Domain.Entities;

namespace PikUpStix.Trading.NTests
{
    [TestFixture]
    class AnnualReturnTests
    {
        [Test]
        public void AnnualReturn_Tests()
        {
            var initialInvestment = 1000;
            var results = LoadTestAnnualReturnData(initialInvestment);
            var annualReturns = new AnnualReturns(1, results,1000);
            Assert.AreEqual(Math.Round(annualReturns.First(x => x.Key == 1980).Value.ReturnAsPercentofInvestmentCapital, 2), 0.37);
            Assert.AreEqual(Math.Round(annualReturns.First(x => x.Key == 1981).Value.ReturnAsPercentofInvestmentCapital, 2), 0.27);
        }

        private List<TestTrade> LoadTestAnnualReturnData(double initialInvestment)
        {
            var noOfDays = 730;
            var startDate = new DateTime(1980, 1, 1, 6, 0, 0);
            var endDate = startDate.AddHours(12);
            var count = 0;
            var results = new List<TestTrade>();
            
            while (count < noOfDays)
            {
                results.Add(new TestTrade() { Margin = 1, CapitalAtEntry = initialInvestment, CapitalAtClose = initialInvestment + 1, Created = startDate, ClosedAt = endDate});
                initialInvestment++;
                startDate = startDate.AddDays(1);
                endDate = startDate.AddHours(12);
                count++;
            }
            return results;
        }
    }
}