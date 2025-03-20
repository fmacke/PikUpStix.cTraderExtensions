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
                    EntryPrice = 1,
                    StopLoss = 1,
                    TakeProfit = 1,
                    Volume = 1,
                    Created = DateTime.Now
                },
                new Position()
                {
                    EntryPrice = 1,
                    StopLoss = 1,
                    TakeProfit = 1,
                    Volume = 1,
                    Created = DateTime.Now
                }
            };
            _closedPositions = _closedPositions = new List<Position>
            {
                new Position()
                    {
                        EntryPrice = 1,
                        StopLoss = 1,
                        TakeProfit = 1,
                        Volume = 1,
                        Created = DateTime.Now
                    },
                new Position()
                    {
                        EntryPrice = 1,
                        StopLoss = 1,
                        TakeProfit = 1,
                        Volume = 1,
                        Created = DateTime.Now
                    }
            };
        }
        [TestMethod]
        public void ClosePositionTest()
        {
            var firstPosition = _openPositions.First();
            var positionHandler = new PositionHandler(
                new List<PositionUpdate> {
                    new PositionUpdate(
                        firstPosition,
                        InstructionType.Close) }, 
                 ref _openPositions, ref _closedPositions);
            positionHandler.ExecuteInstructions();
            Assert.AreEqual(1, _openPositions.Count);
            Assert.AreEqual(3, _closedPositions.Count);

            firstPosition = _openPositions.First();
            positionHandler = new PositionHandler(
                new List<PositionUpdate> {
                    new PositionUpdate(
                        firstPosition,
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
        [TestMethod]
        public void ModifyPositionTest()
        {
            var expectedStopLoss = 2;
            var expectedTakeProfit = 3;
            var firstPosition = _openPositions.First();
            var positionUpdate = new PositionUpdate(
                firstPosition,
                InstructionType.Modify)
            {
                AdjustStopLossTo = expectedStopLoss,
                AdjustTakeProfitTo = expectedTakeProfit
            };
            var positionHandler = new PositionHandler(
               new List<PositionUpdate> { positionUpdate },
                 ref _openPositions, ref _closedPositions);
            positionHandler.ExecuteInstructions();

            var modifiedPosition = _openPositions.First();
            Assert.AreEqual(expectedStopLoss, modifiedPosition.StopLoss);
            Assert.AreEqual(expectedTakeProfit, modifiedPosition.TakeProfit);
        }

    }
}
