using System;
using NUnit.Framework;
using PikUpStix.Trading.Common.PositionSize;
using PikUpStix.Trading.Volatility;


namespace PikUpStix.Trading.NTests
{
    [TestFixture]
    public class PositionSizeTests
    {
        public decimal instrument_Forecast { get; set; }
        public decimal tradingCapital { get; set; }
        public decimal targetVolatility { get; set; }
        public decimal instrumentPrice { get; set; }
        public decimal instrumentPriceVolatility { get; set; }
        public decimal instrumentBlock { get; set; }
        public decimal exchangeRate { get; set; }

        [Test]
        public void Calculate_Position_Size_From_Forecast()
        {
            instrument_Forecast = Convert.ToDecimal(-6.0);
            tradingCapital = 4000000;
            targetVolatility = Convert.ToDecimal(0.25);
            instrumentPrice = Convert.ToDecimal(75);
            instrumentPriceVolatility = Convert.ToDecimal(0.0133);
            instrumentBlock = 1000;
            exchangeRate = Convert.ToDecimal(0.67);
            var insrtumentPositionSize = new InstrumentPositionSize(instrumentBlock, instrumentPrice, instrumentPriceVolatility, exchangeRate);
            var subSystemPosition = new SubSystemPosition(instrument_Forecast, tradingCapital, targetVolatility, insrtumentPositionSize);

            Assert.AreEqual(1000000, subSystemPosition.AnnualisedCashVolatility);
            Assert.AreEqual(62500, subSystemPosition.DailyCashVolatilityTarget);
            Assert.AreEqual(750, subSystemPosition.InstrumentDetails.BlockValue);
            Assert.AreEqual(997.5, subSystemPosition.InstrumentDetails.CurrencyVolatility);
            Assert.AreEqual(668.325, subSystemPosition.InstrumentDetails.ValueVolatility);
            Assert.AreEqual(93.5173755283731717353084203, subSystemPosition.VolatilityScalar);
            Assert.AreEqual(-56.11042531702390304118505218, subSystemPosition.GetUnscaledPosition());
        }

        [Test]
        public void Calculate_Position_Size_From_Forecast_FxProTesting()
        {
            instrument_Forecast = Convert.ToDecimal(20.0);
            tradingCapital = 1000000;
            targetVolatility = Convert.ToDecimal(0.25);
            instrumentPrice = Convert.ToDecimal(0.76014);
            instrumentPriceVolatility = Convert.ToDecimal(0.00379131403653604);
            instrumentBlock = 100000;
            exchangeRate = Convert.ToDecimal(1);
            var insrtumentPositionSize = new InstrumentPositionSize(instrumentBlock, instrumentPrice, instrumentPriceVolatility, exchangeRate);
            var subSystemPosition = new SubSystemPosition(instrument_Forecast, tradingCapital, targetVolatility, insrtumentPositionSize);

            //Assert.AreEqual(1000000, subSystemPosition.AnnualisedCashVolatility);
            //Assert.AreEqual(62500, subSystemPosition.DailyCashVolatilityTarget);
            //Assert.AreEqual(750, subSystemPosition.InstrumentDetails.BlockValue);
            //Assert.AreEqual(997.5, subSystemPosition.InstrumentDetails.CurrencyVolatility);
            //Assert.AreEqual(668.325, subSystemPosition.InstrumentDetails.ValueVolatility);
            //Assert.AreEqual(93.5173755283731717353084203, subSystemPosition.VolatilityScalar);
            Assert.AreEqual(-56.11042531702390304118505218, subSystemPosition.GetUnscaledPosition());
        }
    }
}
