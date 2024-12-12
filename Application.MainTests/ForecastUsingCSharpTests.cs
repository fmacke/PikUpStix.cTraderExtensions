using Application.Business.Forecasts.CarverTrendFollower;
using Domain.Entities;

namespace Application.MainTests
{
    [TestFixture]
    public class ForecastUsingCSharpTests
    {
        private List<HistoricalData> data;
        private List<HistoricalData> excelData;

        [OneTimeSetUp]
        public void Init()
        {
            LoadData();
            LoadExcelData();
        }
        [Test]
        public void Calculate_Single_Unscaled_Forecast()
        {
            if (data == null)
                LoadData();
            var ewmacCSharpForecastValue = new EwmacForecastValue(DateTime.Now, data, 0, 0, new List<Test_Parameter>());
            var forecast16_64 = ewmacCSharpForecastValue.GetUnscaledForecast(16, 64, 36, 3.75);
            var forecastToCheck = forecast16_64.Last();
            Assert.AreEqual(Convert.ToDouble(6.3998), Math.Round(forecastToCheck.Forecast, 4));
        }
        [Test]
        public void Calculate_Combined_Scaled_Capped_Forecast()
        {
            if (excelData == null)
                LoadExcelData();
            var ewmacCSharpForecastValue = new EwmacForecastValue(new DateTime(2017, 1, 20), excelData, 0, 0, new List<Test_Parameter>());
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
            //            dataItem.Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) };
            //            excelData.Add(dataItem);
            //        }
            //    }
            //    else
            //    {
            //        var dataItem = historicalData;
            //        dataItem.InstrumentId = 1; //CME BP FUTURE MINI
            //        dataItem.Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) };
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
                InstrumentId = 1,
                ClosePrice = Convert.ToDecimal(60.77),
                Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) }
            });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 03), InstrumentId = 1, ClosePrice = Convert.ToDecimal(58.31), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 04), InstrumentId = 1, ClosePrice = Convert.ToDecimal(55.65), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 05), InstrumentId = 1, ClosePrice = Convert.ToDecimal(56.29), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 08), InstrumentId = 1, ClosePrice = Convert.ToDecimal(56.08), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 09), InstrumentId = 1, ClosePrice = Convert.ToDecimal(55.65), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 10), InstrumentId = 1, ClosePrice = Convert.ToDecimal(53.95), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 11), InstrumentId = 1, ClosePrice = Convert.ToDecimal(51.91), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 12), InstrumentId = 1, ClosePrice = Convert.ToDecimal(52.96), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 16), InstrumentId = 1, ClosePrice = Convert.ToDecimal(51.23), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 17), InstrumentId = 1, ClosePrice = Convert.ToDecimal(52.3), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 18), InstrumentId = 1, ClosePrice = Convert.ToDecimal(50.51), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 19), InstrumentId = 1, ClosePrice = Convert.ToDecimal(51.98), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 22), InstrumentId = 1, ClosePrice = Convert.ToDecimal(51.11), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 23), InstrumentId = 1, ClosePrice = Convert.ToDecimal(53.61), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 24), InstrumentId = 1, ClosePrice = Convert.ToDecimal(54.24), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 25), InstrumentId = 1, ClosePrice = Convert.ToDecimal(53.49), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 26), InstrumentId = 1, ClosePrice = Convert.ToDecimal(55.38), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 29), InstrumentId = 1, ClosePrice = Convert.ToDecimal(54.01), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 30), InstrumentId = 1, ClosePrice = Convert.ToDecimal(57.03), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 01, 31), InstrumentId = 1, ClosePrice = Convert.ToDecimal(58.17), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 01), InstrumentId = 1, ClosePrice = Convert.ToDecimal(57.35), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 02), InstrumentId = 1, ClosePrice = Convert.ToDecimal(59.01), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 05), InstrumentId = 1, ClosePrice = Convert.ToDecimal(58.69), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 06), InstrumentId = 1, ClosePrice = Convert.ToDecimal(58.91), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 07), InstrumentId = 1, ClosePrice = Convert.ToDecimal(57.75), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 08), InstrumentId = 1, ClosePrice = Convert.ToDecimal(59.76), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 09), InstrumentId = 1, ClosePrice = Convert.ToDecimal(59.86), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 12), InstrumentId = 1, ClosePrice = Convert.ToDecimal(57.76), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 13), InstrumentId = 1, ClosePrice = Convert.ToDecimal(58.98), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 14), InstrumentId = 1, ClosePrice = Convert.ToDecimal(58), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 15), InstrumentId = 1, ClosePrice = Convert.ToDecimal(57.92), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 16), InstrumentId = 1, ClosePrice = Convert.ToDecimal(59.38), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 20), InstrumentId = 1, ClosePrice = Convert.ToDecimal(58.32), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 21), InstrumentId = 1, ClosePrice = Convert.ToDecimal(59.4), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 22), InstrumentId = 1, ClosePrice = Convert.ToDecimal(60.28), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 23), InstrumentId = 1, ClosePrice = Convert.ToDecimal(60.28), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 26), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.41), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 27), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.46), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 02, 28), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.78), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 01), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.97), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 02), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.58), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 05), InstrumentId = 1, ClosePrice = Convert.ToDecimal(60.05), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 06), InstrumentId = 1, ClosePrice = Convert.ToDecimal(60.66), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 07), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.85), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 08), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.63), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 09), InstrumentId = 1, ClosePrice = Convert.ToDecimal(60.06), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 12), InstrumentId = 1, ClosePrice = Convert.ToDecimal(58.94), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 13), InstrumentId = 1, ClosePrice = Convert.ToDecimal(58.03), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 14), InstrumentId = 1, ClosePrice = Convert.ToDecimal(58.15), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 15), InstrumentId = 1, ClosePrice = Convert.ToDecimal(57.52), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 16), InstrumentId = 1, ClosePrice = Convert.ToDecimal(57.06), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 19), InstrumentId = 1, ClosePrice = Convert.ToDecimal(56.65), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 20), InstrumentId = 1, ClosePrice = Convert.ToDecimal(56.41), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 21), InstrumentId = 1, ClosePrice = Convert.ToDecimal(56.98), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 22), InstrumentId = 1, ClosePrice = Convert.ToDecimal(60.21), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 23), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.07), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 26), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.77), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 27), InstrumentId = 1, ClosePrice = Convert.ToDecimal(62.98), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 28), InstrumentId = 1, ClosePrice = Convert.ToDecimal(64.11), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 29), InstrumentId = 1, ClosePrice = Convert.ToDecimal(66.1), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 03, 30), InstrumentId = 1, ClosePrice = Convert.ToDecimal(65.94), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 02), InstrumentId = 1, ClosePrice = Convert.ToDecimal(66.03), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 03), InstrumentId = 1, ClosePrice = Convert.ToDecimal(64.59), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 04), InstrumentId = 1, ClosePrice = Convert.ToDecimal(64.4), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 05), InstrumentId = 1, ClosePrice = Convert.ToDecimal(64.26), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 09), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.51), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 10), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.92), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 11), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.98), Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) } });
            data.Add(new HistoricalData()
            {
                Date = new DateTime(2007, 04, 12),
                InstrumentId = 1,
                ClosePrice = Convert.ToDecimal(63.87),
                Instrument = new Instrument() { MinimumPriceFluctuation = Convert.ToDecimal(0.0001) }
            });
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 13), InstrumentId = 1, ClosePrice = Convert.ToDecimal(63.63),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 16), InstrumentId = 1, ClosePrice = Convert.ToDecimal(63.63),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 17), InstrumentId = 1, ClosePrice = Convert.ToDecimal(63.14),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 18), InstrumentId = 1, ClosePrice = Convert.ToDecimal(63.14),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 19), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.81),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 20), InstrumentId = 1, ClosePrice = Convert.ToDecimal(63.56),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 23), InstrumentId = 1, ClosePrice = Convert.ToDecimal(65.33),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 24), InstrumentId = 1, ClosePrice = Convert.ToDecimal(64.1),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 25), InstrumentId = 1, ClosePrice = Convert.ToDecimal(65.33),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 26), InstrumentId = 1, ClosePrice = Convert.ToDecimal(65.08),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 27), InstrumentId = 1, ClosePrice = Convert.ToDecimal(66.45),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 04, 30), InstrumentId = 1, ClosePrice = Convert.ToDecimal(65.78),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 01), InstrumentId = 1, ClosePrice = Convert.ToDecimal(64.43),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 02), InstrumentId = 1, ClosePrice = Convert.ToDecimal(63.78),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 03), InstrumentId = 1, ClosePrice = Convert.ToDecimal(63.23),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 04), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.89),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 07), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.48),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 08), InstrumentId = 1, ClosePrice = Convert.ToDecimal(62.26),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 09), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.54),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 10), InstrumentId = 1, ClosePrice = Convert.ToDecimal(61.85),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 11), InstrumentId = 1, ClosePrice = Convert.ToDecimal(62.35),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 14), InstrumentId = 1, ClosePrice = Convert.ToDecimal(62.55),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 15), InstrumentId = 1, ClosePrice = Convert.ToDecimal(63.16),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 16), InstrumentId = 1, ClosePrice = Convert.ToDecimal(62.57),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 17), InstrumentId = 1, ClosePrice = Convert.ToDecimal(64.83),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 18), InstrumentId = 1, ClosePrice = Convert.ToDecimal(64.93),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 21), InstrumentId = 1, ClosePrice = Convert.ToDecimal(66.25),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 22), InstrumentId = 1, ClosePrice = Convert.ToDecimal(64.91),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 23), InstrumentId = 1, ClosePrice = Convert.ToDecimal(65.1),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 24), InstrumentId = 1, ClosePrice = Convert.ToDecimal(63.62),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 25), InstrumentId = 1, ClosePrice = Convert.ToDecimal(64.59),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 29), InstrumentId = 1, ClosePrice = Convert.ToDecimal(63.19),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 30), InstrumentId = 1, ClosePrice = Convert.ToDecimal(63.47),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 05, 31), InstrumentId = 1, ClosePrice = Convert.ToDecimal(64.02),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 01), InstrumentId = 1, ClosePrice = Convert.ToDecimal(65.09),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 04), InstrumentId = 1, ClosePrice = Convert.ToDecimal(66.17),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 05), InstrumentId = 1, ClosePrice = Convert.ToDecimal(65.63),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 06), InstrumentId = 1, ClosePrice = Convert.ToDecimal(65.97),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 07), InstrumentId = 1, ClosePrice = Convert.ToDecimal(66.93),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 08), InstrumentId = 1, ClosePrice = Convert.ToDecimal(64.78),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 11), InstrumentId = 1, ClosePrice = Convert.ToDecimal(65.93),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 12), InstrumentId = 1, ClosePrice = Convert.ToDecimal(65.36),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 13), InstrumentId = 1, ClosePrice = Convert.ToDecimal(66.17),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 14), InstrumentId = 1, ClosePrice = Convert.ToDecimal(67.62),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 15), InstrumentId = 1, ClosePrice = Convert.ToDecimal(68.04),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 18), InstrumentId = 1, ClosePrice = Convert.ToDecimal(69.06),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 19), InstrumentId = 1, ClosePrice = Convert.ToDecimal(69.15),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 20), InstrumentId = 1, ClosePrice = Convert.ToDecimal(68.5),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 21), InstrumentId = 1, ClosePrice = Convert.ToDecimal(68.35),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 22), InstrumentId = 1, ClosePrice = Convert.ToDecimal(68.85),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 25), InstrumentId = 1, ClosePrice = Convert.ToDecimal(68.83),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 26), InstrumentId = 1, ClosePrice = Convert.ToDecimal(67.78),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 27), InstrumentId = 1, ClosePrice = Convert.ToDecimal(68.98),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 28), InstrumentId = 1, ClosePrice = Convert.ToDecimal(69.61),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 06, 29), InstrumentId = 1, ClosePrice = Convert.ToDecimal(70.47),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 02), InstrumentId = 1, ClosePrice = Convert.ToDecimal(71.11),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 03), InstrumentId = 1, ClosePrice = Convert.ToDecimal(71.41),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 05), InstrumentId = 1, ClosePrice = Convert.ToDecimal(71.81),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 06), InstrumentId = 1, ClosePrice = Convert.ToDecimal(72.8),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 09), InstrumentId = 1, ClosePrice = Convert.ToDecimal(72.14),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 10), InstrumentId = 1, ClosePrice = Convert.ToDecimal(72.8),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 11), InstrumentId = 1, ClosePrice = Convert.ToDecimal(72.58),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 12), InstrumentId = 1, ClosePrice = Convert.ToDecimal(72.55),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 13), InstrumentId = 1, ClosePrice = Convert.ToDecimal(73.89),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 16), InstrumentId = 1, ClosePrice = Convert.ToDecimal(74.11),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 17), InstrumentId = 1, ClosePrice = Convert.ToDecimal(74.03),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 18), InstrumentId = 1, ClosePrice = Convert.ToDecimal(75.03),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 19), InstrumentId = 1, ClosePrice = Convert.ToDecimal(75.9),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 20), InstrumentId = 1, ClosePrice = Convert.ToDecimal(75.53),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 23), InstrumentId = 1, ClosePrice = Convert.ToDecimal(74.65),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 24), InstrumentId = 1, ClosePrice = Convert.ToDecimal(73.38),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 25), InstrumentId = 1, ClosePrice = Convert.ToDecimal(75.74),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 26), InstrumentId = 1, ClosePrice = Convert.ToDecimal(74.96),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 27), InstrumentId = 1, ClosePrice = Convert.ToDecimal(77.03),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 30), InstrumentId = 1, ClosePrice = Convert.ToDecimal(76.82),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 07, 31), InstrumentId = 1, ClosePrice = Convert.ToDecimal(78.2),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 08, 01), InstrumentId = 1, ClosePrice = Convert.ToDecimal(76.49),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 08, 02), InstrumentId = 1, ClosePrice = Convert.ToDecimal(76.84),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
            //data.Add(new HistoricalData() { Date = new DateTime(2007, 08, 03), InstrumentId = 1, ClosePrice = Convert.ToDecimal(75.41),Instrument = new Instrument(){MinimumPriceFluctuation = Convert.ToDecimal(0.0001)}});
        }
    }
}