using Application.Business.BackTest.Reports;
using Domain.Entities;

namespace Application.MainTests
{
    [TestFixture]
    class SharpeRatioTests
    {
        [Test]
        public void SharpeRatio_Tests()
        {
            var results = LoadTestData();
            var sharpeRatio = new SharpeRatio(results);

            Assert.AreEqual(Math.Round(5.18571, 5), Math.Round(sharpeRatio.AveragePnL, 5));
            Assert.AreEqual(Math.Round(2.66856, 5), Math.Round(sharpeRatio.StandardDeviationOfPnL, 5));
            Assert.AreEqual(Math.Round(1.9433, 5), Math.Round(sharpeRatio.Value, 5));
        }
        private List<TestTrade> LoadTestData()
        {
            return new List<TestTrade>()
            {
                new TestTrade(){ Margin = 1},
                new TestTrade(){ Margin = 1.5},
                new TestTrade(){ Margin = 5},
                new TestTrade(){ Margin = 6},
                new TestTrade(){ Margin = 7},
                new TestTrade(){ Margin = 8},
                new TestTrade(){ Margin = 7.8}
            };
        }


    }
}
