namespace PikUpStix.Trading.NTests
{
    [TestFixture]
    [Ignore("Don't call Quandl during routine unit testing")]
    public class ImportQuandlDataTests
    {
        [Test]
        public void Import_Recent_Historical_Data_From_Quandl_ToSql_Test()
        {
            //var x = new QuandlDataImport();
            //var pullDataFrom = new DateTime(1974, 1, 1);
            //var db = new TraderDBContextDerived();
            //var instruments = db.Instruments.Where(xe => xe.Provider == "QUANDL");
            //x.ImportLatestData(instruments.ToList(), pullDataFrom); /// TODO: Populate instrument list
        }
        [Test]
        public void Import_Historical_Data_From_Quandl_ToSql_Test()
        {
            //var reImportDataFromQuandlToSqlServer = false;
            //if (reImportDataFromQuandlToSqlServer)
            //{
            //   // This is housed within an IF statement to prevent large and slow data imports happening everytime 
            //   // tests are run
            //    var x = new QuandlDataImport();
            //    x.ClearExistingAndImportNewData(false);
            //}
        }
        [Test]
        public void Get_Exchange_Rate_From_Quandl_ToSql_Test()
        {
            //  var x = QuandlConnector.GetCurrentExchangeRate(DateTime.Now.AddDays(-50), CurrencyPair.GBPUSD);
            //var y = x;
        }
        
    }
}
