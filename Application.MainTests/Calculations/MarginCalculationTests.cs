using Application.Business.Calculations;
using DocumentFormat.OpenXml.Bibliography;
using Domain.Entities;
using Domain.Enums;
using System.Diagnostics;

namespace Application.Tests.Calculations
{
    [TestFixture]
    public class MarginCalculationTests
    {
        [Test]
        public void CalculateCurrentPositionPositiveMarginTest()
        {
            var entryPrice = 77.00;
            var closePrice = 78.52;
            var contractUnit = 500;
            var volume = 2.5;
            var exchangeRate = 0.77;
            var lotSize = 1;

            var trade = new Position();
            trade.EntryPrice = entryPrice;
            trade.ClosePrice = closePrice;
            trade.PositionType = PositionType.BUY;
            trade.Volume = volume;

            var margin = new Margin(lotSize, contractUnit, exchangeRate, trade, closePrice, trade.Volume).Calculate();
            Debug.Write("Margin Anticipated: £1463, Margin Actual: £" + margin);
            Assert.AreEqual(1463, Math.Round(margin,0));
        }
        [Test]
        public void MarginTestTwo()
        {
            var entryPrice = 1.28791;
            var closePrice = 1.38791;
            var contractUnit = 0.0001;
            var volume = 5000;
            var exchangeRate = 1;
            var lotSize = 100000;

            var trade = new Position();
            trade.EntryPrice = entryPrice;
            trade.ClosePrice = closePrice;
            trade.PositionType = PositionType.BUY;
            trade.Volume = volume;

            var margin = new Margin(lotSize, contractUnit, exchangeRate, trade, closePrice, trade.Volume).Calculate();
            Debug.Write("Margin Anticipated: £1463, Margin Actual: £" + margin);
            Assert.AreEqual(5000    , Math.Round(margin, 0));
        }

        [Test]
        public void MarginTest3()
        {
            var entryPrice = 1;
            var closePrice = 1.001;
            var contractUnit = 0.0001;
            var volume = 1000;
            var exchangeRate = 1;
            var lotSize = 100000;

            var trade = new Position();
            trade.EntryPrice = entryPrice;
            trade.ClosePrice = closePrice;
            trade.PositionType = PositionType.BUY;
            trade.Volume = volume;

            var margin = new Margin(lotSize, contractUnit, exchangeRate, trade, closePrice, trade.Volume).Calculate();
            Debug.Write("Margin Anticipated: £1463, Margin Actual: £" + margin);
            Assert.AreEqual(10, Math.Round(margin, 0));
        }

    }
}