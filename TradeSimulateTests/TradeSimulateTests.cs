using Domain.Entities;
using Domain.Enums;
using Robots.Common;
using TradeSimulator;

namespace TradeSimulateTests
{
    [TestClass]
    public sealed class PositionHandlerTests
    {
        List<Position> _openPositions;
        List<Position> _closedPositions;

        [TestInitialize]
        public void TestInitialize()
        {
            _openPositions = new List<Position>
            {
                new Position()
                {
                    Id = 3,
                    EntryPrice = 1,
                    StopLoss = 1,
                    TakeProfit = 1,
                    Volume = 1
                },
                new Position()
                {
                    Id = 4,
                    EntryPrice = 1,
                    StopLoss = 1,
                    TakeProfit = 1,
                    Volume = 1
                }
            };
            _closedPositions = _closedPositions = new List<Position>
            {
                new Position()
                    {
                        Id = 1,
                        EntryPrice = 1,
                        StopLoss = 1,
                        TakeProfit = 1,
                        Volume = 1
                    },
                new Position()
                    {
                        Id = 2,
                        EntryPrice = 1,
                        StopLoss = 1,
                        TakeProfit = 1,
                        Volume = 1
                    }
            };
        }
        [TestMethod]
        public void ClosePositionTest()
        {
             var positionHandler = new PositionHandler(
                new List<PositionUpdate> { 
                    new PositionUpdate(
                        new Position
                        {
                            Id = 4
                        }, 
                        InstructionType.Close) }, 
                ref _openPositions, ref _closedPositions);
            positionHandler.ExecuteInstructions();
            Assert.AreEqual(1, _openPositions.Count);
            Assert.AreEqual(3, _closedPositions.Count);


            positionHandler = new PositionHandler(
                new List<PositionUpdate> {
                    new PositionUpdate(
                        new Position
                        {
                            Id = 3
                        },
                        InstructionType.Close) },
                ref _openPositions, ref _closedPositions);
            positionHandler.ExecuteInstructions();
            Assert.AreEqual(0, _openPositions.Count);
            Assert.AreEqual(4, _closedPositions.Count);
        }

        [TestMethod]
        public void OpenPositionTest()
        {
            var positionHandler = new PositionHandler(
               new List<PositionUpdate> {
                    new PositionUpdate(
                        new Position
                        {
                            EntryPrice = 1,
                            StopLoss = 1,
                            Created = DateTime.Now
                        },
                        InstructionType.Open) },
               ref _openPositions, ref _closedPositions);
            positionHandler.ExecuteInstructions();
            Assert.AreEqual(3, _openPositions.Count);
            Assert.AreEqual(2, _closedPositions.Count);


            positionHandler = new PositionHandler(
                new List<PositionUpdate> {
                    new PositionUpdate(
                        new Position
                        {
                            EntryPrice = 1,
                            StopLoss = 1,
                            Created = DateTime.Now
                        },
                        InstructionType.Open) },
                ref _openPositions, ref _closedPositions);
            positionHandler.ExecuteInstructions();
            Assert.AreEqual(4, _openPositions.Count);
            Assert.AreEqual(2, _closedPositions.Count);
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
