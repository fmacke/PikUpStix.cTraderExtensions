using Application.Business.Volatility;
using Domain.Entities;

namespace PikUpStix.Trading.NTests
{
    [TestFixture]
    public class VolatilityTests
    {
        private List<HistoricalData> data;
        [OneTimeSetUp]
        public void Init()
        {
            LoadData();
        }

        private void LoadData()
        {
            //data = ExcelConnector.GethHistoricalPrices(@"C:\dev\PikUpStix.Trading\PikUpStix.Trading.NTests\CsvData\CME_BP1 - British Pound Futures (Front Month) to 20170122 - SMALL.xlsx");
        }

        [Test]
        public void GetInstrumentVolatility()
        {
            if(data == null)
            LoadData();
            var priceVol = new PriceVolatility(data, 25);
            var stdDev = priceVol.StandardDeviation;
            //Assert.AreEqual(true, true) NEED TO GET THIS WORKING AND DETST THAT VOLATIILITY SCALAR WORKING PROPERLY
        }
        [Test]
        public void Test_Standard_Deviation_Calc()
        {
            var dataSample = new List<HistoricalData>
            {
                new HistoricalData() {ClosePrice = -10},
                new HistoricalData() {ClosePrice =1},
                new HistoricalData() {ClosePrice = 10},
                new HistoricalData() {ClosePrice = 20},
                new HistoricalData() {ClosePrice = 30}
            };
            var priceVol = new PriceVolatility(dataSample, 5);
            var stdDev = priceVol.CalculateStdDev(new Double[5]{-10,0,10,20,30});
            var stdDev2 = priceVol.CalculateStdDev(new Double[5] { 8,9,10,11,12 });
            Assert.AreEqual(14.142135623730951, stdDev);
        }
    }
}
