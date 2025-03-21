using Domain.Entities;
using System.Diagnostics;
using TradeSimulator;

namespace TradeSimulateTests
{
    [TestClass]
    public sealed class TestSimulation
    {
        List<HistoricalData> Bars;

        [TestInitialize]
        public void TestInitialize()
        {
            DateTime startDate = new DateTime(2000, 1, 1);                
                
            Bars = new List<HistoricalData> ();
            int expander = 1;
            int direction = 1;
            int subCounter = 6;
            for (int x = 0; x < 200; x++)
            {                
                Bars.Add(new HistoricalData()
                {
                    Date = startDate.AddDays(x),
                    OpenPrice = subCounter,
                    ClosePrice = subCounter + expander,
                    HighPrice = subCounter + (2*expander),
                    LowPrice = subCounter - (2*expander),
                });
                if (subCounter % 5 == 0 || (subCounter == 1 && direction == -1))
                {
                    if (direction > 0)
                    {
                        direction = -1;
                    }
                    else
                    {
                        direction = 1;
                    }
                }
                subCounter += direction;
                Debug.WriteLine($"Date: {Bars[x].Date} Open: {Bars[x].OpenPrice} Close: {Bars[x].ClosePrice} High: {Bars[x].HighPrice} Low: {Bars[x].LowPrice}");
            }

        }
        [TestMethod]
        public void RunSimulation()
        {
            var tradeSimulator = new TradeSimulate(Bars);
            tradeSimulator.Run();
            Assert.IsNotNull(tradeSimulator.CurrentBar.HighPrice);
        }
    }
}
