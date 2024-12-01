using System;
using System.IO;
using System.Data.Entity.Migrations;
using System.Linq;
using NUnit.Framework;
using PikUpStix.Trading.Common.Extensions;
using PikUpStix.Trading.Data;
using PikUpStix.Trading.Data.Local.SqlDb;

namespace PikUpStix.Trading.NTests
{
    [TestFixture]    
    public class ImportDukasCopyExcelTests
    {
        [Test]
        [Ignore("For data import only")]
        public void Import_DukasCopy_Data_from_Excel()
        {
            DirectoryInfo d = new DirectoryInfo(@"C:\Users\Finn\OneDrive\Documents\Business\trading\HistoricalData");
            FileInfo[] Files = d.GetFiles("*.csv"); 
            string str = "";
            foreach (FileInfo file in Files)
            {
                var historicalData = ExcelConnector.GethHistoricalPricesFromDukasCopyFile(file.FullName);

                try
                {
                    var instrument = new Instrument()
                    {
                        InstrumentName = MiscExtensions.GetUntilOrEmpty(file.Name, "_"),
                        Provider = "DUKASCOPY",
                        DataName = MiscExtensions.GetUntilOrEmpty(file.Name, "_"),
                        DataSource = "DUKASCOPY",
                        Format = "CSV",
                        Frequency = "DAILY",
                        Sort = "DESC",
                        ContractUnit = 1,
                        ContractUnitType = 1.ToString(),
                        PriceQuotation = "Spot Currency Rate",
                        MinimumPriceFluctuation = Convert.ToDecimal(0.0001),
                        Currency = "USD",
                    };
                    var db = new TraderDBContextDerived();
                    var instruments = db.Instruments.Where(x => x.Provider == instrument.Provider && x.Frequency == instrument.Frequency & x.InstrumentName == instrument.InstrumentName);
                    
                    if (instruments.Any())
                    {
                        instrument.InstrumentId = instruments.First().InstrumentId;
                    }
                    instrument.HistoricalDatas = historicalData;

                    db.Instruments.AddOrUpdate(instrument);
                    db.SaveChanges();
                    //foreach (var data in historicalData)
                    //    data.InstrumentId = instrument.InstrumentId;

                    //db.HistoricalDatas.AddRange(historicalData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message.ToString());
                }
            }
        }
    }
}
