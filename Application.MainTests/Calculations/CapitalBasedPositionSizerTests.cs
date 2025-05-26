using Application.Business.Calculations;

namespace Application.Tests.Calculations
{
    [TestFixture]
    public class CapitalBasedPositionSizerTests
    {
        private double forecast;
        private double maximumRisk;
        private double accountBalance;
        private double pipSize;
        private double lotSize;

        [SetUp]
        public void Setup()
        {
            // Common setup values can be initialized here if needed
        }

        [Test]
        public void CalculatePositionSize_ForexEURUSD()
        {
            forecast = 1;
            maximumRisk = 0.02;
            accountBalance = 10000;
            pipSize = 0.0001;
            lotSize = 100000;

            var result = Math.Round(new CapitalBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize).Calculate(), 4);
            Assert.AreEqual(20, result);
        }

        [Test]
        public void CalculatePositionSize_GoldXAUUSD()
        {
            forecast = 1;
            maximumRisk = 0.03;
            accountBalance = 50000;
            pipSize = 0.10;
            lotSize = 100;

            Assert.AreEqual(150, Math.Round(new CapitalBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize).Calculate(), 2));
        }

        [Test]
        public void CalculatePositionSize_StockAAPL()
        {
            forecast = 1;
            maximumRisk = 0.05;
            accountBalance = 25000;
            pipSize = 0.01;
            lotSize = 1;
            //  125000 seems a very high number - probably worth checking this is actually right
            Assert.AreEqual(125000, Math.Round(new CapitalBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize).Calculate(), 2));
        }

        [Test]
        public void CalculatePositionSize_CryptoBTCUSD()
        {
            forecast = 1;
            maximumRisk = 0.10;
            accountBalance = 15000;
            pipSize = 1;
            lotSize = 1;

            Assert.AreEqual(1500, Math.Round(new CapitalBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize).Calculate(), 2));
        }

        [Test]
        public void CalculateLotSizeTest()
        {
            forecast = 1;
            maximumRisk = 0.02;
            accountBalance = 10000;
            pipSize = 1;
            lotSize = 100000;

            Assert.AreEqual(Math.Round(0.002, 10), Math.Round(new CapitalBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize).Calculate(), 10));

            forecast = 1;
            maximumRisk = 0.1;
            accountBalance = 10000;
            pipSize = 1;
            lotSize = 100000;

            Assert.AreEqual(Math.Round(0.01, 5), Math.Round(new CapitalBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize).Calculate(), 5));
        }
    }
}
