using System;
using NUnit.Framework;
using PikUpStix.Trading.Forecast.Reports;
using PikUpStix.Trading.Common.Enums;
using PikUpStix.Trading.Data.Local.SqlDb;

namespace PikUpStix.Trading.NTests
{
    [TestFixture]
    public class PositionTests
    {
        [Test]
        public void CalculateCurrentPositionPositiveMarginTest()
        {
            var purchasePrice = (decimal)77;
            var closePrice = (decimal)78.52;
            var contractUnit = 500;
            var noOfContracts = 2.5;
            var exchangeRate = (decimal) 0.77;

            var trade = new Test_Trades();
            trade.EntryPrice = purchasePrice;
            trade.ClosePrice = closePrice;
            trade.Direction = PositionType.BUY.ToString();
            trade.Volume = (decimal)noOfContracts;

            var margin = Margin.Calculate(contractUnit, exchangeRate, trade, closePrice, trade.Volume);
            Console.WriteLine("Margin Anticipated: £1463, Margin Actual: £" + margin);
            Assert.AreEqual(Convert.ToDecimal(1463), margin);
        }
    }
}