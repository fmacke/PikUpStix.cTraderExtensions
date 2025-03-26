using Application.Business.Calculations;
using Domain.Entities;

namespace Application.MainTests.Calculations
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

        private List<Position> LoadTestAnnualReturnData(double initialInvestment)
        {
            var noOfDays = 730;
            var startDate = new DateTime(1980, 1, 1, 6, 0, 0);
            var endDate = startDate.AddHours(12);
            var count = 0;
            var results = new List<Position>();
            
            while (count < noOfDays)
            {
                results.Add(new Position() { Margin = 1,Created = startDate, ClosedAt = endDate});
                initialInvestment++;
                startDate = startDate.AddDays(1);
                endDate = startDate.AddHours(12);
                count++;
            }
            return results;
        }
    }
}