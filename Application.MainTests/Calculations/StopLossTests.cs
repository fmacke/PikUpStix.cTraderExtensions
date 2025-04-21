using Application.Business.Calculations;
using Application.Business.Risk;
using Domain.Enums;
namespace Application.Tests.Calculations
{
    [TestFixture]
    public class TrailingStopLossTests
    {
        [Test]
        public void CalculateValidBuyTrailingStopLoss()
        {
            var tradeType = PositionType.BUY;
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
            var tradeType = PositionType.SELL;
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
            var tradeType = PositionType.BUY;
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
            var tradeType = PositionType.SELL;
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
            var tradeType = PositionType.BUY;
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
            var tradeType = PositionType.SELL;
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
            var tradeType = PositionType.BUY;
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
            var tradeType = PositionType.SELL;
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
        public void CalculateStopLossPrice()
        {
            var existingMargin = 100;
            var stopLossMaxPercent = 0.02;
            var contractUnit = 1;
            var volume = 1;
            var exchangeRate = 1;
            var posType = PositionType.BUY;
            var purchasePrice = 10;
            var minimumPriceFluctuation = 0.0001;

            var stopLossCalc = new StopLossAtPrice(existingMargin, stopLossMaxPercent, contractUnit,
                volume, exchangeRate, posType, purchasePrice, purchasePrice, minimumPriceFluctuation);
            var stopLossPrice = stopLossCalc.Calculate();
            Assert.AreEqual(8, stopLossPrice);

            posType = PositionType.SELL;
            stopLossCalc = new StopLossAtPrice(existingMargin, stopLossMaxPercent, contractUnit,
                volume, exchangeRate, posType, purchasePrice, purchasePrice, minimumPriceFluctuation);
            stopLossPrice = stopLossCalc.Calculate();
            Assert.AreEqual(12, stopLossPrice);
        }
        [Test]
        public void CalculateStopLossPriceUsingRealValues()
        {
            var existingMargin = 2000;
            var stopLossMaxPercent = 0.02;
            var contractUnit = 6250;
            var volume = 1;
            var exchangeRate = 1.377;
            var posType = PositionType.BUY;
            var purchasePrice = 1;
            var minimumPriceFluctuation = 0.0001;

            var stopLoss = new StopLossAtPrice(existingMargin, stopLossMaxPercent, contractUnit,
                volume, exchangeRate, posType, purchasePrice, purchasePrice, minimumPriceFluctuation);
            var stopLossInCurrency = stopLoss.Calculate();
            Assert.AreEqual(Convert.ToDouble(0.9954), Convert.ToDouble(Math.Round(stopLossInCurrency, 4)));

            posType = PositionType.SELL;
            stopLoss = new StopLossAtPrice(existingMargin, stopLossMaxPercent, contractUnit,
                volume, exchangeRate, posType, purchasePrice, purchasePrice, minimumPriceFluctuation);
            stopLossInCurrency = stopLoss.Calculate();
            Assert.AreEqual(1.0046, stopLossInCurrency);
        }
    }
}
