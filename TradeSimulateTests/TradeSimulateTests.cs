using Domain.Entities;
using Domain.Enums;
using TradeSimulator;

namespace TradeSimulateTests
{
    [TestClass]
    public sealed class TradeSimulateTests
    {
        [TestMethod]
        public void RunSimpleTestSimulationTest()
        {
            var tradeSimulate = new TradeSimulate(GetData());
            tradeSimulate.RunTradeSimulation();
        }

        [TestMethod]
        public void ClosePositionTest()
        {
            var positions = new List<TestTrade>
            {
                new TestTrade()
                {
                    Id = 1,
                    EntryPrice = 1,
                    StopLoss = 1,
                    TakeProfit = 1,
                    Volume = 1
                },
                new TestTrade()
                {
                    Id = 2,
                    EntryPrice = 1,
                    StopLoss = 1,
                    TakeProfit = 1,
                    Volume = 1
                }
            };
            var trades = new List<TestTrade>
            {
                new TestTrade()
                {
                    Id = 3,
                    EntryPrice = 1,
                    StopLoss = 1,
                    TakeProfit = 1,
                    Volume = 1
                },
                new TestTrade()
                {
                    Id = 4,
                    EntryPrice = 1,
                    StopLoss = 1,
                    TakeProfit = 1,
                    Volume = 1
                }
            };
            PositionHandler.ClosePosition(ref positions, ref trades, positions[0]);
            Assert.AreEqual(1, positions.Count);
            Assert.AreEqual(3, trades.Count);
            PositionHandler.ClosePosition(ref positions, ref trades, positions[0]);
            Assert.AreEqual(0, positions.Count);
            Assert.AreEqual(4, trades.Count);
        }

        private List<HistoricalData> GetData()
        {
            var bars = new List<HistoricalData>();
            var cursorDate = DateTime.Now;
            for (int i = 0; i < 10; i++)
            {
                bars.Add(new HistoricalData
                {
                    Date = cursorDate,
                    OpenPrice = i,
                    ClosePrice = i,
                    LowPrice = i,
                    HighPrice = i,
                    Volume = i,
                    Settle = i,
                    OpenInterest = i,
                    InstrumentId = 1
                });
                cursorDate = cursorDate.AddDays(1);
            }
            return bars;
        }
    }
}
