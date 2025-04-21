using Application.Business.Calculations;

namespace Application.Tests.Calculations
{
    [TestFixture]
    public class LotSizeTests
    {
        public double forecast { get; set; }
        public double maximumRisk { get; set; }
        public double accountBalance { get; set; }
        public double pipSize { get; set; }
        public double stopLossPrice { get; set; }
        public double entryPrice { get; set; }

        [SetUp]
        public void Setup()
        {
            forecast = 1;
            maximumRisk = 0.02;
            accountBalance = 10000;
            pipSize = 1;
            stopLossPrice = 8;
            entryPrice = 10;
        }
        [Test]
        public void CalculateLotSizeTest()
        {
            Assert.AreEqual(100, Math.Round(new LotSize(forecast, maximumRisk, accountBalance, pipSize, stopLossPrice, entryPrice).Calculate(),4));
            forecast = 1;
            maximumRisk = 0.1;
            accountBalance = 10000;
            pipSize = 1;
            stopLossPrice = 35;
            entryPrice = 50;
            Assert.AreEqual(66.6667, Math.Round(new LotSize(forecast, maximumRisk, accountBalance, pipSize, stopLossPrice, entryPrice).Calculate(), 4));
        }
       
    }
}
