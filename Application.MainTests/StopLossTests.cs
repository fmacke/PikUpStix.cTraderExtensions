using Application.Business;
using Application.RiskControl;
using Domain.Enums;
namespace Application.MainTests
{
    [TestFixture]
    public class TrailingStopLossTests
    {
        [Test]
        public void CalculateValidBuyTrailingStopLoss()
        {
            var tradeType = TradeType.Buy;
            var entryPrice = 10;
            var originalStopLossAt = 8;
            var currentPrice = 15;
            var executeTrailAt = 4;
            var moveTrailingStopBy = 2;
            var trailingStop = new TrailingStop(tradeType, entryPrice, originalStopLossAt, 0, currentPrice, executeTrailAt, moveTrailingStopBy);
            Assert.AreEqual(true, trailingStop.TrailingStopUpdated);
            Assert.AreEqual(12, trailingStop.TrailingStopAt);
        }
        [Test]
        public void CalculateValidSellTrailingStopLoss()
        {
            var tradeType = TradeType.Sell;
            var entryPrice = 10;
            var originalStopLossAt = 12;
            var currentPrice = 5;
            var executeTrailAt = 4;
            var moveTrailingStopBy = 2;
            var trailingStop = new TrailingStop(tradeType, entryPrice, originalStopLossAt, currentPrice, 0, executeTrailAt, moveTrailingStopBy);
            Assert.AreEqual(true, trailingStop.TrailingStopUpdated);
            Assert.AreEqual(8, trailingStop.TrailingStopAt);
        }
        [Test]
        public void CalculateBuyGainTooSmallTrailingStopLoss()
        {
            var tradeType = TradeType.Buy;
            var entryPrice = 10;
            var originalStopLossAt = 8;
            var currentPrice = 13;
            var executeTrailAt = 4;
            var moveTrailingStopBy = 2;
            var trailingStop = new TrailingStop(tradeType, entryPrice, originalStopLossAt, 0, currentPrice, executeTrailAt, moveTrailingStopBy);
            Assert.AreEqual(false, trailingStop.TrailingStopUpdated);
            Assert.AreEqual(8, trailingStop.TrailingStopAt);
        }
        [Test]
        public void CalculateSellGainTooSmallTrailingStopLoss()
        {
            var tradeType = TradeType.Sell;
            var entryPrice = 10;
            var originalStopLossAt = 12;
            var currentPrice = 7;
            var executeTrailAt = 4;
            var moveTrailingStopBy = 2;
            var trailingStop = new TrailingStop(tradeType, entryPrice, originalStopLossAt, currentPrice, 0, executeTrailAt, moveTrailingStopBy);
            Assert.AreEqual(false, trailingStop.TrailingStopUpdated);
            Assert.AreEqual(12, trailingStop.TrailingStopAt);
        }
        [Test]
        public void NoBuyTrailingStopIfLossBeingIncurred()
        {
            var tradeType = TradeType.Buy;
            var entryPrice = 10;
            var originalStopLossAt = 8;
            var currentPrice = 9;
            var executeTrailAt = 4;
            var moveTrailingStopBy = 2;
            var trailingStop = new TrailingStop(tradeType, entryPrice, originalStopLossAt, 0, currentPrice, executeTrailAt, moveTrailingStopBy);
            Assert.AreEqual(false, trailingStop.TrailingStopUpdated);
            Assert.AreEqual(8, trailingStop.TrailingStopAt);
        }
        [Test]
        public void NoSellTrailingStopIfLossBeingIncurred()
        {
            var tradeType = TradeType.Sell;
            var entryPrice = 10;
            var originalStopLossAt = 12;
            var currentPrice = 11;
            var executeTrailAt = 4;
            var moveTrailingStopBy = 2;
            var trailingStop = new TrailingStop(tradeType, entryPrice, originalStopLossAt, currentPrice, 0, executeTrailAt, moveTrailingStopBy);
            Assert.AreEqual(false, trailingStop.TrailingStopUpdated);
            Assert.AreEqual(12, trailingStop.TrailingStopAt);
        }
        [Test]
        public void NoBuyTrailingStopIfAlreadyBeyondTrailStopLimit()
        {
            var tradeType = TradeType.Buy;
            var entryPrice = 10;
            var currentStopLoss = 12;
            var currentPrice = 18;
            var executeTrailAt = 4;
            var moveTrailingStopBy = 2;
            var trailingStop = new TrailingStop(tradeType, entryPrice, currentStopLoss, 0, currentPrice, executeTrailAt, moveTrailingStopBy);
            Assert.AreEqual(false, trailingStop.TrailingStopUpdated);
            Assert.AreEqual(12, trailingStop.TrailingStopAt);
        }
        [Test]
        public void NoSellTrailingStopIfAlreadyBeyondTrailStopLimit()
        {
            var tradeType = TradeType.Sell;
            var entryPrice = 10;
            var currentStopLoss = 8;
            var currentPrice = 2;
            var executeTrailAt = 4;
            var moveTrailingStopBy = 2;
            var trailingStop = new TrailingStop(tradeType, entryPrice, currentStopLoss, currentPrice, 0, executeTrailAt, moveTrailingStopBy);
            Assert.AreEqual(false, trailingStop.TrailingStopUpdated);
            Assert.AreEqual(8, trailingStop.TrailingStopAt);
        }
    }
    [TestFixture]
    public class StopLossTests

    {
        [Test]
        public void CalculateStopLossInCurrency()
        {
            var existingMargin = 100;
            var stopLossMaxPercent = 0.02;
            var contractUnit = 1;
            var volume = 1;
            var exchangeRate = 1;
            var posType = PositionType.BUY;
            var purchasePrice = 10;
            var minimumPriceFluctuation = 0.0001;

            var stopLoss = new StopLoss(existingMargin, stopLossMaxPercent, contractUnit,
                volume, exchangeRate, posType, purchasePrice, purchasePrice, minimumPriceFluctuation);
            var stopLossInCurrency = stopLoss.StopLossInCurrency();
            Assert.AreEqual(8, stopLossInCurrency);

            posType = PositionType.SELL;
            volume = -1;

            stopLoss = new StopLoss(existingMargin, stopLossMaxPercent, contractUnit,
                volume, exchangeRate, posType, purchasePrice, purchasePrice, minimumPriceFluctuation);
            stopLossInCurrency = stopLoss.StopLossInCurrency();
            Assert.AreEqual(12, stopLossInCurrency);
        }
        [Test]
        public void CalculateStopLossInCurrencyUsingRealValues()
        {
            var existingMargin = 2000;
            var stopLossMaxPercent = 0.02;
            var contractUnit = 6250;
            var volume = 1;
            var exchangeRate = 1.377;
            var posType = PositionType.BUY;
            var purchasePrice = 1;
            var minimumPriceFluctuation = 0.0001;

            var stopLoss = new StopLoss(existingMargin, stopLossMaxPercent, contractUnit,
                volume, exchangeRate, posType, purchasePrice, purchasePrice, minimumPriceFluctuation);
            var stopLossInCurrency = stopLoss.StopLossInCurrency();
            Assert.AreEqual(0.9936, stopLossInCurrency);

            posType = PositionType.SELL;
            volume = -1;
            stopLoss = new StopLoss(existingMargin, stopLossMaxPercent, contractUnit,
                volume, exchangeRate, posType, purchasePrice, purchasePrice, minimumPriceFluctuation);
            stopLossInCurrency = stopLoss.StopLossInCurrency();
            Assert.AreEqual(1.0064, stopLossInCurrency);
        }
    }
}
