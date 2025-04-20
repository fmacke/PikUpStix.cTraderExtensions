using Application.Business.Risk;
using Domain.Enums;

namespace Application.Tests
{
    [TestFixture]
    public class RiskManagerTests
    {
        public double forecast { get; set; }
        public double maximumRisk { get; set; }
        public double accountBalance { get; set; }
        public double pipSize { get; set; }
        public double stopLoss { get; set; }
        public double entryPrice { get; set; }

        [SetUp]
        public void Setup()
        {
            forecast = 1;
            maximumRisk = 0.02;
            accountBalance = 10000;
            pipSize = 1;
            stopLoss = 8;
            entryPrice = 10;
        }
        [Test]
        public void CalculateRiskTest()
        {
            var riskManager = new RiskManager(forecast, maximumRisk, accountBalance, pipSize, stopLoss, entryPrice);
            var lotSize = riskManager.CalculateLotSize();
            Assert.AreEqual(0.2, lotSize);
        }
       
    }
}
