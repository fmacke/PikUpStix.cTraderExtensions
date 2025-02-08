using Application.Business.BackTest.Reports;
using Domain.Entities;
using Domain.Enums;

namespace Application.MainTests
{
    [TestFixture]
    public class PositionTests
    {
        [Test]
        public void CalculateCurrentPositionPositiveMarginTest()
        {
            var purchasePrice = 77.00;
            var closePrice = 78.52;
            var contractUnit = 500;
            var noOfContracts = 2.5;
            var exchangeRate = 0.77;

            var trade = new TestTrade();
            trade.EntryPrice = purchasePrice;
            trade.ClosePrice = closePrice;
            trade.Direction = PositionType.BUY.ToString();
            trade.Volume = noOfContracts;

            var margin = Margin.Calculate(contractUnit, exchangeRate, trade, closePrice, trade.Volume);
            Console.WriteLine("Margin Anticipated: £1463, Margin Actual: £" + margin);
            Assert.AreEqual(Convert.ToDecimal(1463), margin);
        }
    }
}