using QuandlCS.Connection;
using QuandlCS.Requests;
using QuandlCS.Types;

namespace PikUpStix.Trading.Data.Quandl
{
    public enum CurrencyPair
    {
       GBPUSD
    }
    public static class QuandlConnector
    {
        //private static int LimitTestResultsTo = 50;

        //public static List<List<HistoricalData>> GetData(bool testOnly, DateTime startDate, List<Instrument> instruments)
        //{
        //    return instruments.Select(instrument => GetHistoricalData(testOnly, startDate, instrument)).ToList();
        //}

        //private static List<HistoricalData> GetHistoricalData(bool testOnly, DateTime startDate, Instrument instrument)
        //{
        //    var request = new QuandlDownloadRequest
        //    {
        //        APIKey = ConfigurationManager.AppSettings["QuandlAPIKey"],
        //        Datacode = new Datacode(instrument.DataSource, instrument.DataName),
        //        Format = FileFormats.JSON,
        //        Frequency = Frequencies.Daily,
        //        StartDate = startDate,
        //        Sort = SortOrders.Descending
        //    };
        //    if (testOnly)
        //        request.Truncation = LimitTestResultsTo;

        //    var conn = new QuandlConnection();
        //    string json = conn.Request(request);
        //    var obj = JsonConvert.DeserializeObject<RootObject>(json);
        //    var data = new List<HistoricalData>();
        //    foreach (var item in obj.data)
        //    {
        //        if (item.Count == 2)
        //        {
        //            //  Exchange Rate data
        //            data.Add(new HistoricalData
        //            {
        //                Date = Convert.ToDateTime(item[0]),
        //                OpenPrice = Convert.ToDecimal(item[1])
        //            });
        //        }
        //        else
        //        {
        //            // Standard Instrument Data
        //            data.Add(new HistoricalData
        //            {
        //                Date = Convert.ToDateTime(item[0]),
        //                OpenPrice = Convert.ToDecimal(item[1]),
        //                HighPrice = Convert.ToDecimal(item[2]),
        //                LowPrice = Convert.ToDecimal(item[3]),
        //                ClosePrice = Convert.ToDecimal(item[4]),
        //                Volume = Convert.ToDecimal(item[7]),
        //                Settle = Convert.ToDecimal(item[6]),
        //                OpenInterest = Convert.ToDecimal(item[8]),
        //                InstrumentId = instrument.InstrumentId
        //            });
        //        }
        //    }
        //    return data.OrderByDescending(x => x.Date).ToList();
        //}
    }
}