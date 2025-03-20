using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Bars = new List<HistoricalData> ();
            for(int x = 0; x < 100; x++)
            {
                Bars.Add(new HistoricalData()
                {
                    Date = DateTime.Now.AddDays(x),
                    OpenPrice = x,
                    ClosePrice = x,
                    HighPrice = x,
                    LowPrice = x
                });
            }
        }
        [TestMethod]
        public void RunSimulation()
        {
            var tradeSimulator = new TradeSimulate(Bars);
            Assert.IsNotNull(tradeSimulator.ClosedTrades);
        }
    }
}
