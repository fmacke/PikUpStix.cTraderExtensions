using Application.Business.Error;
using Domain.Entities;
using PikUpStix.Trading.Data;
using PikUpStix.Trading.Data.Quandl;

namespace Application.Business.Utilities.Quandl
{
    public class QuandlDataImport //: IDataImport
    {
        //private readonly TraderDBContextDerived db = new TraderDBContextDerived();

        public void ImportLatestData(List<Instrument> instruments, DateTime pullDataFrom)
        {

            foreach (Instrument quandlDetail in instruments)
            {

                //if (db.HistoricalDatas.Where(x => x.InstrumentId == quandlDetail.Id).Max(x => x.Date).HasValue)
                //    pullDataFrom =
                //        Convert.ToDateTime(
                //            db.HistoricalDatas.Where(x => x.InstrumentId == quandlDetail.Id).Max(x => x.Date))
                //            .AddDays(1);
                ImportHistoricalValues(quandlDetail, false, pullDataFrom);
            }
            UpdateExchangeRates();
        }

        public void UpdateExchangeRates()
        {
            var pullDataFrom = new DateTime(1974, 1, 1);
            //if (db.HistoricalDatas.Any(x => x.InstrumentId == 11))// InstrumentId 11 = GBPUSD exchange rate
            //{

            //    pullDataFrom =
            //        Convert.ToDateTime(
            //            db.HistoricalDatas.Where(x => x.InstrumentId == 11).Max(x => x.Date))
            //            .AddDays(1);
            //}
            //ImportHistoricalValues(db.Instruments.First(x => x.InstrumentId == 11), false, pullDataFrom);
        }

        public void ClearExistingAndImportNewData(bool limitImport)
        {
            //List<Instrument> futureHistoriesForDownload = db.Instruments.ToList();

            //foreach (Instrument quandlDetail in futureHistoriesForDownload)
            //{
            //    DeleteExistingHistory(quandlDetail);
            //    ImportHistoricalValues(quandlDetail, limitImport, new DateTime(1974, 1, 1));
            //}
        }

        private void DeleteExistingHistory(Instrument quandlDetail)
        {
            //db.Database.ExecuteSqlCommand("Delete from HistoricalData where InstrumentID = {0}",
            //    new object[] { quandlDetail.InstrumentId });
        }

        private void ImportHistoricalValues(Instrument instrumentDetails, bool limitImport, DateTime startDate)
        {
            var instrument = new List<Instrument>
            {
                instrumentDetails
            };
            try
            {
                //List<List<HistoricalData>> data = QuandlConnector.GetData(limitImport, startDate, instrument);
                //var historicalsToAdd = new List<HistoricalData>();
                //foreach (HistoricalData historicalPrice in data.First())
                //{
                //    var day = new HistoricalData
                //    {
                //        InstrumentId = instrumentDetails.InstrumentId,
                //        Date = historicalPrice.Date,
                //        OpenPrice = historicalPrice.OpenPrice,
                //        ClosePrice = historicalPrice.ClosePrice,
                //        LowPrice = historicalPrice.LowPrice,
                //        HighPrice = historicalPrice.HighPrice,
                //        Volume = historicalPrice.Volume,
                //        Settle = historicalPrice.Settle,
                //        OpenInterest = historicalPrice.OpenInterest
                //    };
                //    historicalsToAdd.Add(day);
                //}
                //db.HistoricalDatas.AddRange(historicalsToAdd);
                //db.SaveChanges();
            }
            catch (Exception ex)
            {
                var error = new ErrorHandler("ImportQuandlDataTests", "ImportHistoricalValues", ex);
            }
        }
    }
}