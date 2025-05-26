using Application.Business.Calculations;

namespace Application.Tests.Calculations
{
    [TestFixture]
    public class StopLossPositionSizeTests
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
        public void CalculatePositionSize_ForexEURUSD()
        {
            forecast = 1;
            maximumRisk = 0.02;
            accountBalance = 10000;
            pipSize = 0.0001;
            lotSize = 100000;
            stopLossPrice = 1.09143;
            entryPrice = 1.09243;
            var result = Math.Round(new StopLossBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize, stopLossPrice, entryPrice).Calculate(), 4);
            Assert.AreEqual(2, result);
        }
        [Test]
        public void CalculatePositionSize_GoldXAUUSD()
        {
            double forecast = 1;
            double maximumRisk = 0.03;
            double accountBalance = 50000;
            double pipSize = 0.10; // Gold moves in increments of $0.10
            double lotSize = 100; // Standard futures contract size
            double stopLossPrice = 1980;
            double entryPrice = 2000;
            // co-pilot reckons this should be 7.5 standard lots rather than 0.75
            Assert.AreEqual(0.75, Math.Round(new StopLossBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize, stopLossPrice, entryPrice).Calculate(), 2));
        }
        [Test]
        public void CalculatePositionSize_StockAAPL()
        {
             forecast = 1;
             maximumRisk = 0.05;
             accountBalance = 25000;
             pipSize = 0.01;  // Tick size for stock trading
             lotSize = 1; // Shares are traded in whole units
             stopLossPrice = 150;
             entryPrice = 155;

            Assert.AreEqual(250, Math.Round(new StopLossBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize, stopLossPrice, entryPrice).Calculate(), 2));
        }
        [Test]
        public void CalculatePositionSize_CryptoBTCUSD()
        {
            double forecast = 1;
            double maximumRisk = 0.10; // Higher risk tolerance due to volatility
            double accountBalance = 15000;
            double pipSize = 1; // BTC/USD moves in full dollar increments
            double lotSize = 1; // 1 BTC per contract
            double stopLossPrice = 38000;
            double entryPrice = 40000;
            Assert.AreEqual(0.75, Math.Round(new StopLossBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize, stopLossPrice, entryPrice).Calculate(), 2));
        }
        [Test]
        public void CalculateLotSizeTest()
        {
            forecast = 1;
            maximumRisk = 0.02;
            accountBalance = 10000;
            pipSize = 0.0001;
            lotSize = 100000;
            stopLossPrice = 1.2;
            entryPrice = 1.3;
            Assert.AreEqual(0.02, new StopLossBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize, stopLossPrice, entryPrice).Calculate());
            
            forecast = 1;
            maximumRisk = 0.02;
            accountBalance = 10000;
            pipSize = 0.0001;
            lotSize = 100000;
            stopLossPrice = 1.65;
            entryPrice = 1.7;
            Assert.AreEqual(0.04, new StopLossBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize, stopLossPrice, entryPrice).Calculate());

            forecast = 0.5;
            maximumRisk = 0.02;
            accountBalance = 10000;
            pipSize = 0.0001;
            lotSize = 100000;
            stopLossPrice = 1.65;
            entryPrice = 1.7;
            Assert.AreEqual(0.02, new StopLossBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize, stopLossPrice, entryPrice).Calculate());

            forecast = 1;
            lotSize = 100000;
            maximumRisk = 0.04;
            accountBalance = 6781.73;
            pipSize = 0.0001;
            stopLossPrice = 1.09143;
            entryPrice = 1.09243;
            Assert.AreEqual(Math.Round(2.713,3), Math.Round(new StopLossBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize, stopLossPrice, entryPrice).Calculate(),3));
            
            forecast = 0.73398719064586015;
            lotSize = 100000;
            maximumRisk = 0.05;
            accountBalance = 10000;
            pipSize = 0.0001;
            stopLossPrice = 1.1897;
            entryPrice = 1.2897;
            Assert.AreEqual(Math.Round(0.036699359532293009,3),
                Math.Round(new StopLossBasedPositionSizer(forecast, maximumRisk, accountBalance, lotSize, pipSize, stopLossPrice, entryPrice).Calculate(), 3));
        }

    }
}
