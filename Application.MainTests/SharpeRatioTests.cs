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
            Assert.AreEqual(Math.Round(1.9433, 5), Math.Round(sharpeRatio.Get, 5));
        }
        private List<Test_Trades> LoadTestData()
        {
            return new List<Test_Trades>()
            {
                new Test_Trades(){ Margin = 1},
                new Test_Trades(){ Margin = Convert.ToDecimal(1.5)},
                new Test_Trades(){ Margin = 5},
                new Test_Trades(){ Margin = 6},
                new Test_Trades(){ Margin = 7},
                new Test_Trades(){ Margin = 8},
                new Test_Trades(){ Margin = Convert.ToDecimal(7.8)}
            };
        }


    }
}
