using Application.Business.Calculations;
using Domain.Entities;

namespace Application.Tests.Calculations
{
    [TestFixture]
    class SharpeRatioTests
    {
        [Test]
        public void SharpeRatio_Tests()
        {
            var results = LoadTestData();
            var sharpeRatio = new SharpeRatio(results);

            //Assert.AreEqual(Math.Round(5.18571, 5), Math.Round(sharpeRatio.AveragePnL, 5));
            //Assert.AreEqual(Math.Round(2.66856, 5), Math.Round(sharpeRatio.StandardDeviationOfPnL, 5));
            Assert.AreEqual(Math.Round(1.9433, 5), Math.Round(sharpeRatio.Calculate(), 5));
        }
        private List<Position> LoadTestData()
        {
            return new List<Position>()
            {
                new Position(){ Margin = 1},
                new Position(){ Margin = 1.5},
                new Position(){ Margin = 5},
                new Position(){ Margin = 6},
                new Position(){ Margin = 7},
                new Position(){ Margin = 8},
                new Position(){ Margin = 7.8}
            };
        }


    }
}
