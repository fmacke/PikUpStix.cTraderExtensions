using Application.Business.PositionSize;

namespace Application.Tests
{
    [TestFixture]
    public class PositionSizeTests
    {
        public double instrument_Forecast { get; set; }
        public double tradingCapital { get; set; }
        public double targetVolatility { get; set; }
        public double instrumentPrice { get; set; }
        public double instrumentPriceVolatility { get; set; }
        public double instrumentBlock { get; set; }
        public double exchangeRate { get; set; }

        [Test]
        public void Calculate_Position_Size_From_Forecast()
        {
            instrument_Forecast = -6.0;
            tradingCapital = 4000000;
            targetVolatility = 0.25;
            instrumentPrice = 75;
            instrumentPriceVolatility = 0.0133;
            instrumentBlock = 1000;
            exchangeRate = 0.67;
            var insrtumentPositionSize = new InstrumentPositionSize(instrumentBlock, instrumentPrice, instrumentPriceVolatility, exchangeRate);
            var subSystemPosition = new SubSystemPosition(instrument_Forecast, tradingCapital, targetVolatility, insrtumentPositionSize);

            Assert.AreEqual(1000000, subSystemPosition.AnnualisedCashVolatility);
            Assert.AreEqual(62500, subSystemPosition.DailyCashVolatilityTarget);
            Assert.AreEqual(750, subSystemPosition.InstrumentDetails.BlockValue);
            Assert.AreEqual(997.5, subSystemPosition.InstrumentDetails.CurrencyVolatility);
            Assert.AreEqual(668.325, subSystemPosition.InstrumentDetails.ValueVolatility);
            Assert.AreEqual(93.5173755283731717353084203, subSystemPosition.VolatilityScalar);
            Assert.AreEqual(-56.110425317023896, subSystemPosition.GetUnscaledPosition());
        }

        [Test]
        public void Calculate_Position_Size_From_Forecast_FxProTesting()
        {
            instrument_Forecast = 20.0;
            tradingCapital = 1000000;
            targetVolatility = 0.25;
            instrumentPrice = 0.76014;
            instrumentPriceVolatility = 0.00379131403653604;
            instrumentBlock = 100000;
            exchangeRate = 1;
            var insrtumentPositionSize = new InstrumentPositionSize(instrumentBlock, instrumentPrice, instrumentPriceVolatility, exchangeRate);
            var subSystemPosition = new SubSystemPosition(instrument_Forecast, tradingCapital, targetVolatility, insrtumentPositionSize);
            
            Assert.AreEqual(250000, subSystemPosition.AnnualisedCashVolatility);
            Assert.AreEqual(15625, subSystemPosition.DailyCashVolatilityTarget);
            Assert.AreEqual(760.14, Math.Round(subSystemPosition.InstrumentDetails.BlockValue, 4));
            Assert.AreEqual(288.1929, Math.Round(subSystemPosition.InstrumentDetails.CurrencyVolatility,4));
            Assert.AreEqual(288.1929, Math.Round(subSystemPosition.InstrumentDetails.ValueVolatility,4));
            Assert.AreEqual(54.2171, Math.Round(subSystemPosition.VolatilityScalar,4));
            Assert.AreEqual(108.434, Math.Round(subSystemPosition.GetUnscaledPosition(),3));
        }
    }
}
