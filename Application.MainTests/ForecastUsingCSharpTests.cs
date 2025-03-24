using Application.Business;
using Application.Business.Forecasts.CarverTrendFollower;
using Application.Business.Market;
using Domain.Entities;

namespace Application.MainTests
{
    [TestFixture]
    public class ForecastUsingCSharpTests
    {
        private MarketInfo currentMarketInfo;
        private List<HistoricalData> data;
        private List<HistoricalData> excelData;

        [OneTimeSetUp]
        public void Init()
        {
            LoadData();
            LoadExcelData();
            currentMarketInfo = new MarketInfo(new DateTime(2017, 1, 20), 1.2345, 1.2346, new List<Position>(),
                data, "CME BP FUTURE MINI", "GBP", 10000, 0.0001);
        }
        [Test]
        public void Calculate_Single_Unscaled_Forecast()
        {
            if (data == null)
                LoadData();
            var ewmacCSharpForecastValue = new EwmacForecastValue(currentMarketInfo, new List<Test_Parameter>());
            var forecast16_64 = ewmacCSharpForecastValue.GetUnscaledForecast(16, 64, 36, 3.75);
            var forecastToCheck = forecast16_64.Last();
            Assert.AreEqual(Convert.ToDouble(6.3998), Math.Round(forecastToCheck.Forecast, 4));
        }
        [Test]
        public void Calculate_Combined_Scaled_Capped_Forecast()
        {
            if (excelData == null)
                LoadExcelData();
            var ewmacCSharpForecastValue = new EwmacForecastValue(currentMarketInfo, new List<Test_Parameter>());
            //Assert.AreEqual(-13.320916565243922m, ewmacCSharpForecastValue.CalculateForecast());
            // Note this is a slightly different number to the Python version because it calculates forecast starting with less priming data
            Assert.AreEqual(-12.736708587895151m, ewmacCSharpForecastValue.CalculateForecast());
        }

        //private void DebugPrint(List<ForecastElement> forecastElements)
        //{
        //    var str = new StringBuilder();
        //    var foreCastScaling = new ForecastScaling();
        //    Console.Write(foreCastScaling.WriteOutData(forecastElements));
        //}

        private void LoadExcelData()
        {
            //excelData = new List<HistoricalData>();
            //var dat = ExcelConnector.GethHistoricalPrices(@"C:\dev\PikUpStix.Trading\PikUpStix.Trading.NTests\CsvData\CME_BP1 - British Pound Futures (Front Month) to 20170122 - SMALL.xlsx");
            //foreach (var historicalData in dat)
            //{
            //    if (historicalData.Date.Value.Year == 2014 && historicalData.Date.Value.Month == 1)
            //    {
            //        if (historicalData.Date.Value.Day > 14)
            //        {
            //            var dataItem = historicalData;
            //            dataItem.InstrumentId = 1; //CME BP FUTURE MINI
            //            dataItem.Instrument = new Instrument() { MinimumPriceFluctuation = 0.0001) };
            //            excelData.Add(dataItem);
            //        }
            //    }
            //    else
            //    {
            //        var dataItem = historicalData;
            //        dataItem.InstrumentId = 1; //CME BP FUTURE MINI
            //        dataItem.Instrument = new Instrument() { MinimumPriceFluctuation = 0.0001) };
            //        excelData.Add(dataItem);
            //    }
            //}
        }

        private void LoadData()
        {
            data = new List<HistoricalData>();
            data.Add(new HistoricalData()
            {
                Date = new DateTime(2007, 1, 2),
                //InstrumentId = 1,
                ClosePrice = 60.77//,
                //Instrument = new Instrument() { MinimumPriceFluctuation = 0.0001) }
            });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 03), ClosePrice = 58.31 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 04), ClosePrice = 55.65 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 05), ClosePrice = 56.29 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 08), ClosePrice = 56.08 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 09), ClosePrice = 55.65 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 10), ClosePrice = 53.95 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 11), ClosePrice = 51.91 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 12), ClosePrice = 52.96 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 16), ClosePrice = 51.23 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 17), ClosePrice = 52.3 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 18), ClosePrice = 50.51 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 19), ClosePrice = 51.98 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 22), ClosePrice = 51.11 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 23), ClosePrice = 53.61 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 24), ClosePrice = 54.24 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 25), ClosePrice = 53.49 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 26), ClosePrice = 55.38 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 29), ClosePrice = 54.01 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 30), ClosePrice = 57.03 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 31), ClosePrice = 58.17 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 01), ClosePrice = 57.35 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 02), ClosePrice = 59.01 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 05), ClosePrice = 58.69 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 06), ClosePrice = 58.91 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 07), ClosePrice = 57.75 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 08), ClosePrice = 59.76 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 09), ClosePrice = 59.86 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 12), ClosePrice = 57.76 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 13), ClosePrice = 58.98 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 14), ClosePrice = 58 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 15), ClosePrice = 57.92 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 16), ClosePrice = 59.38 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 20), ClosePrice = 58.32 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 21), ClosePrice = 59.4 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 22), ClosePrice = 60.28 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 23), ClosePrice = 60.28 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 26), ClosePrice = 61.41 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 27), ClosePrice = 61.46 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 28), ClosePrice = 61.78 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 01), ClosePrice = 61.97 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 02), ClosePrice = 61.58 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 05), ClosePrice = 60.05 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 06), ClosePrice = 60.66 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 07), ClosePrice = 61.85 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 08), ClosePrice = 61.63 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 09), ClosePrice = 60.06 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 12), ClosePrice = 58.94 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 13), ClosePrice = 58.03 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 14), ClosePrice = 58.15 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 15), ClosePrice = 57.52 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 16), ClosePrice = 57.06 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 19), ClosePrice = 56.65 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 20), ClosePrice = 56.41 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 21), ClosePrice = 56.98 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 22), ClosePrice = 60.21 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 23), ClosePrice = 61.07 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 26), ClosePrice = 61.77 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 27), ClosePrice = 62.98 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 28), ClosePrice = 64.11 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 29), ClosePrice = 66.1 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 30), ClosePrice = 65.94 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 02), ClosePrice = 66.03 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 03), ClosePrice = 64.59 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 04), ClosePrice = 64.4 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 05), ClosePrice = 64.26 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 09), ClosePrice = 61.51 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 10), ClosePrice = 61.92 });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 11), ClosePrice = 61.98 });
            data.Add(new HistoricalData()
            {
                Date = new DateTime(2007, 04, 12),
                //InstrumentId = 1,
                ClosePrice = 63.87//,
                //Instrument = new Instrument() { MinimumPriceFluctuation = 0.0001) }
            });
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 13), ClosePrice = 63.63),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 16), ClosePrice = 63.63),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 17), ClosePrice = 63.14),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 18), ClosePrice = 63.14),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 19), ClosePrice = 61.81),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 20), ClosePrice = 63.56),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 23), ClosePrice = 65.33),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 24), ClosePrice = 64.1),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 25), ClosePrice = 65.33),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 26), ClosePrice = 65.08),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 27), ClosePrice = 66.45),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 30), ClosePrice = 65.78),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 01), ClosePrice = 64.43),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 02), ClosePrice = 63.78),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 03), ClosePrice = 63.23),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 04), ClosePrice = 61.89),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 07), ClosePrice = 61.48),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 08), ClosePrice = 62.26),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 09), ClosePrice = 61.54),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 10), ClosePrice = 61.85),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 11), ClosePrice = 62.35),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 14), ClosePrice = 62.55),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 15), ClosePrice = 63.16),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 16), ClosePrice = 62.57),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 17), ClosePrice = 64.83),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 18), ClosePrice = 64.93),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 21), ClosePrice = 66.25),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 22), ClosePrice = 64.91),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 23), ClosePrice = 65.1),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 24), ClosePrice = 63.62),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 25), ClosePrice = 64.59),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 29), ClosePrice = 63.19),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 30), ClosePrice = 63.47),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 31), ClosePrice = 64.02),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 01), ClosePrice = 65.09),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 04), ClosePrice = 66.17),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 05), ClosePrice = 65.63),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 06), ClosePrice = 65.97),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 07), ClosePrice = 66.93),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 08), ClosePrice = 64.78),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 11), ClosePrice = 65.93),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 12), ClosePrice = 65.36),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 13), ClosePrice = 66.17),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 14), ClosePrice = 67.62),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 15), ClosePrice = 68.04),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 18), ClosePrice = 69.06),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 19), ClosePrice = 69.15),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 20), ClosePrice = 68.5),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 21), ClosePrice = 68.35),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 22), ClosePrice = 68.85),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 25), ClosePrice = 68.83),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 26), ClosePrice = 67.78),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 27), ClosePrice = 68.98),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 28), ClosePrice = 69.61),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 29), ClosePrice = 70.47),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 02), ClosePrice = 71.11),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 03), ClosePrice = 71.41),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 05), ClosePrice = 71.81),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 06), ClosePrice = 72.8),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 09), ClosePrice = 72.14),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 10), ClosePrice = 72.8),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 11), ClosePrice = 72.58),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 12), ClosePrice = 72.55),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 13), ClosePrice = 73.89),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 16), ClosePrice = 74.11),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 17), ClosePrice = 74.03),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 18), ClosePrice = 75.03),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 19), ClosePrice = 75.9),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 20), ClosePrice = 75.53),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 23), ClosePrice = 74.65),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 24), ClosePrice = 73.38),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 25), ClosePrice = 75.74),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 26), ClosePrice = 74.96),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 27), ClosePrice = 77.03),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 30), ClosePrice = 76.82),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 31), ClosePrice = 78.2),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 08, 01), ClosePrice = 76.49),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 08, 02), ClosePrice = 76.84),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 08, 03), ClosePrice = 75.41),Instrument = new Instrument(){MinimumPriceFluctuation = 0.0001)}});
        }
    }
}