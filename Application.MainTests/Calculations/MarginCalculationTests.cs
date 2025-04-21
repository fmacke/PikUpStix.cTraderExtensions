using Application.Business.Calculations;
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
            var purchasePrice = 77.00;
            var closePrice = 78.52;
            var contractUnit = 500;
            var noOfContracts = 2.5;
            var exchangeRate = 0.77;

            var trade = new Position();
            trade.EntryPrice = purchasePrice;
            trade.ClosePrice = closePrice;
            trade.PositionType = PositionType.BUY;
            trade.Volume = noOfContracts;

            var margin = new Margin(contractUnit, exchangeRate, trade, closePrice, trade.Volume).Calculate();
            Debug.Write("Margin Anticipated: £1463, Margin Actual: £" + margin);
            Assert.AreEqual(1463, Math.Round(margin,0));
        }
    }
}