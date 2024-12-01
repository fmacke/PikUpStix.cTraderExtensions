namespace PikUpStix.Trading.NTests
{
    [TestFixture]
    [Ignore("Don't call IB Data Import tests during routine unit testing")]
    public class ImportIBDataTests
    {
        [Test]
        public void Import_Recent_Historical_Data_From_InteractiveBrokers_ToSql_Test()
        {
            //var x = new QuandlDataImport();

           // x.ImportLatestData(new List<Instrument>()); /// TODO: Populate instrument list
        }
        [Test]
        public void Import_Historical_Data_From_InteractiveBrokers_ToSql_Test()
        {
            var reImportDataFromQuandlToSqlServer = true;
            if (reImportDataFromQuandlToSqlServer)
            {
                // This is housed within an IF statement to prevent large and slow data imports happening everytime 
                // tests are run
               // var dataImporter = new IBDataImport();
               // var db = new TraderDBContextDerived();
               //var instrumentsToUpdate = new TraderDBContextDerived().Instruments.Where(x => x.InstrumentId == 26).ToList();
               // dataImporter.ImportLatestData(instrumentsToUpdate, new DateTime(2018, 2, 2));
               // finished here!!!!!!!!!!! untested
            }
        }
        [Test]
        public void Get_Exchange_Rate_From_Quandl_ToSql_Test()
        {
            //  var x = QuandlConnector.GetCurrentExchangeRate(DateTime.Now.AddDays(-50), CurrencyPair.GBPUSD);
            //var y = x;
        }

    }
}
