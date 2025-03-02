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
            data = new List<HistoricalData>
            {
                new HistoricalData() {ClosePrice = 1 },
                new HistoricalData() {ClosePrice = 2},
                new HistoricalData() {ClosePrice = 3},
                new HistoricalData() {ClosePrice = 4},
                new HistoricalData() {ClosePrice = 5},
                new HistoricalData() {ClosePrice = 6},
                new HistoricalData() {ClosePrice = 7},
                new HistoricalData() {ClosePrice = 8},
                new HistoricalData() {ClosePrice = 9},
                new HistoricalData() {ClosePrice = 10},
                new HistoricalData() {ClosePrice = 11},
                new HistoricalData() {ClosePrice = 12},
                new HistoricalData() {ClosePrice = 13},
                new HistoricalData() {ClosePrice = 14},
                new HistoricalData() {ClosePrice = 15},
                new HistoricalData() {ClosePrice = 16},
                new HistoricalData() {ClosePrice = 17},
                new HistoricalData() {ClosePrice = 18},
                new HistoricalData() {ClosePrice = 19},
                new HistoricalData() {ClosePrice = 20},
                new HistoricalData() {ClosePrice = 21},
                new HistoricalData() {ClosePrice = 22},
                new HistoricalData() {ClosePrice = 23},
                new HistoricalData() {ClosePrice = 24},
                new HistoricalData() {ClosePrice = 25},
                new HistoricalData() {ClosePrice = 26},
            };
        }

        [Test]
        public void GetInstrumentVolatility()
        {
            if (data == null)
                LoadData();
            var priceVol = new PriceVolatility(data, 25);
            var stdDev = priceVol.StandardDeviation;
            Assert.AreEqual(0.20514688811905316,stdDev);
        }
        [Test]
        public void Test_Standard_Deviation_Calc()
        {            
            var priceVol = new PriceVolatility(data, 5);
            var stdDev = priceVol.CalculateStdDev(new Double[5]{-10,0,10,20,30});
            var stdDev2 = priceVol.CalculateStdDev(new Double[5] { 8,9,10,11,12 });
            Assert.AreEqual(14.142135623730951, stdDev);
        }
    }
}
