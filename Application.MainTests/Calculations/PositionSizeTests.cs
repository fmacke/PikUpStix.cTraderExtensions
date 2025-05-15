using Application.Business.Calculations;

namespace Application.Tests.Calculations
{
    [TestFixture]
    public class PositionSizeTests
    {
        public double forecast { get; set; }
        public double maximumRisk { get; set; }
        public double accountBalance { get; set; }
        public double pipSize { get; set; }
        public double stopLossPrice { get; set; }
        public double entryPrice { get; set; }
        public double lotSize { get; set; }

        [SetUp]
        public void Setup()
        {
            
        }
        [Test]
        public void CalculateLotSizeTest()
        {
            forecast = 1;
            maximumRisk = 0.02;
            accountBalance = 10000;
            pipSize = 1;
            lotSize = 100000;
            stopLossPrice = 8;
            entryPrice = 10;
            Assert.AreEqual(100, Math.Round(new PositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize, stopLossPrice, entryPrice).Calculate(),4));
            forecast = 1;
            maximumRisk = 0.1;
            accountBalance = 10000;
            pipSize = 1;
            stopLossPrice = 35;
            entryPrice = 50;
            Assert.AreEqual(66.6667, Math.Round(new PositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize, stopLossPrice, entryPrice).Calculate(), 4));
            forecast = 1;
            maximumRisk = 0.04;
            accountBalance = 6781.73;
            pipSize = 0.0001;
            stopLossPrice = 1.09143;
            entryPrice = 1.09243;
            Assert.AreEqual(27.127, Math.Round(new PositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize, stopLossPrice, entryPrice).Calculate(), 4));
        }
       
    }
}
