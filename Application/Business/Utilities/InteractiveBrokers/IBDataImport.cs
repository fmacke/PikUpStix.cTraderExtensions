//using Domain.Entities;
//using Application.Business.Error;
//using System.Globalization;

//namespace PikUpStix.Trading.Data.InteractiveBrokers
//{
//    public class IBDataImport : IDataImport
//    {
//        private readonly TraderDBContextDerived db = new TraderDBContextDerived();

//        public void ImportLatestData(List<Instrument> instruments, DateTime pullDataFrom)
//        {
//            //foreach (Instrument instrument in instruments)
//            //{
//            //    if (db.HistoricalDatas.Where(x => x.InstrumentId == instrument.Id).Max(x => x.Date).HasValue)
//            //        pullDataFrom =
//            //            Convert.ToDateTime(
//            //                db.HistoricalDatas.Where(x => x.InstrumentId == instrument.Id).Max(x => x.Date))
//            //                .AddDays(1);
//            //    ImportHistoricalValues(instrument, false, pullDataFrom);
//            //}
//            UpdateExchangeRates();
//        }

//        public void UpdateExchangeRates()
//        {
//            // This function should update exchange rates for the system - which are hard coded right now
//        }

//        private void ImportHistoricalValues(Instrument instrumentDetails, bool limitImport, DateTime startDate)
//        {
//            var instrument = new List<Instrument>
//            {
//                instrumentDetails
//            };
//            try
//            {
//                List<List<HistoricalData>> data = GetData(limitImport, startDate, instrument);
//                var historicalsToAdd = new List<HistoricalData>();
//                foreach (HistoricalData historicalPrice in data.First())
//                {
//                    var day = new HistoricalData
//                    {
//                        InstrumentId = instrumentDetails.Id,
//                        Date = historicalPrice.Date,
//                        OpenPrice = historicalPrice.OpenPrice,
//                        ClosePrice = historicalPrice.ClosePrice,
//                        LowPrice = historicalPrice.LowPrice,
//                        HighPrice = historicalPrice.HighPrice,
//                        Volume = historicalPrice.Volume,
//                        Settle = historicalPrice.Settle,
//                        OpenInterest = historicalPrice.OpenInterest
//                    };
//                    historicalsToAdd.Add(day);
//                }
//                //db.HistoricalDatas.AddRange(historicalsToAdd);
//                //db.SaveChanges();
//            }
//            catch (Exception ex)
//            {
//                var error = new ErrorHandler("IBDataImport", "ImportHistoricalValues", ex);
//            }
//        }

//        private List<List<HistoricalData>> GetData(bool limitImport, DateTime startDate, List<Instrument> instruments)
//        {
//            var data = new List<List<HistoricalData>>();
//            foreach (var ins in instruments)
//            {
//                if (ins.Provider == "INTERACTIVE_BROKERS")
//                {
//                    var application = new IBCommunication();
//                    var daysToPoll = DateTime.Now.Subtract(startDate).Days;
//                    application.RequestHistoricalData(ins.Format, "", ins.DataSource, ins.Currency, ins.DataName, DateTime.Now, daysToPoll.ToString() + " D", "1 day");
//                    var historicalData = application.PrintHistoricalDataMessages();
//                    application.Disconnect();
//                    data.Add(ConvertData(ins.InstrumentId, historicalData));
//                }
//            }
//            return data;
//        }

//        private List<HistoricalData> ConvertData(int instrumentId, List<HistoricalDataMessage> historicalData)
//        {            
//            var historicalsToAdd = new List<HistoricalData>();
//            foreach (var message in historicalData)
//                historicalsToAdd.Add(new HistoricalData
//                {
//                    Date = DateTime.ParseExact(message.Date, "yyyyMMdd", CultureInfo.InvariantCulture),
//                    OpenPrice = Convert.ToDecimal(message.Open),
//                    HighPrice = Convert.ToDecimal(message.High),
//                    LowPrice = Convert.ToDecimal(message.Low),
//                    ClosePrice = Convert.ToDecimal(message.Close),
//                    Volume = Convert.ToDecimal(message.Volume),
//                    Settle = Convert.ToDecimal(0),
//                    OpenInterest = Convert.ToDecimal(0),
//                    InstrumentId = instrumentId
//                });
//            return historicalsToAdd;
//        }

//        public void ClearExistingAndImportNewData(bool limitImport)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
