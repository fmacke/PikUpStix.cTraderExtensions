
namespace Application.Tests
{
    [TestFixture]
    [Ignore("Only useful when connected to Interactive Brokers")]
    public class IBGatewayTests
    {
        //[Test]
        //public void GetMarketData()
        //{
        //    // This only works during market open hours
        //    var application = new IBCommunication();
        //    application.RequestMarketData("CASH", "", "IDEALPRO", "USD", "GBP.USD", DateTime.Now);
        //    Assert.Greater(application.mostRecentTickPrice, 0);
        //    application.Disconnect();
        //}
        //[Test]
        //public void CheckHistoricDataHasLoaded()
        //{
        //    var application = new IBCommunication();
        //    Assert.AreEqual(application.HasHistoricDataLoaded(), false);
        //    application.RequestHistoricalData("CASH", "", "IDEALPRO", "USD", "GBP.USD", DateTime.Now, "128 D", "1 day");
        //    Assert.AreNotEqual(application.HasHistoricDataLoaded(), 0);
        //    application.Disconnect();
        //}
        //[Test]
        //public void GetHistoricalData()
        //{
        //    var application = new IBCommunication();
        //    Assert.AreEqual(application.HasHistoricDataLoaded(), false);
        //    application.RequestHistoricalData("CASH", "", "IDEALPRO", "USD", "GBP.USD", DateTime.Now, "128 D", "1 day");
        //    Assert.Greater(application.PrintHistoricalDataMessages().Count, 0);
        //    application.Disconnect();
        //}
        //[Test]
        //public void GetAccountSummary()
        //{
        //    var application = new IBCommunication();
        //    application.GetAccountSummary();

        //    Console.WriteLine("Account Value is {0:C}", application.AccountValue);
        //    Assert.Greater(application.AccountValue.ToString(), 0.ToString(),
        //        "Account value is " + application.ToString());
        //    application.AccountValueUpdated = false;
        //    application.Disconnect();

        //}
    }
}