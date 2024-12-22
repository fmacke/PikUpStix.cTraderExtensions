using Application.Business.BackTest.Reports;
using Domain.Entities;

namespace Application.MainTests
{
    [TestFixture]
    public class MaximumAdverseExcursionTests
    {
        [Test]
        public void MaxDailyAdverseExcursionTest()
        {
            //todo: this needs updated since Test_Results table was made redundant
            var results = new List<TestTrade>()
            {
                new TestTrade()
                {
                    Margin = 100,
                    //CumulativeMargin = 1100
                },
                new TestTrade()
                {
                    Margin = -200,
                    //CumulativeMargin = 1100
                }
            };
            var mae = new DailyExcursions(results);

            Assert.AreEqual(Math.Round(0.10, 2), Math.Round(mae.MaxFavourableExcursion, 2));
            Assert.AreEqual(Math.Round(-0.15, 2), Math.Round(mae.MaxAdverseExcursion, 2));
        }
    }


}
