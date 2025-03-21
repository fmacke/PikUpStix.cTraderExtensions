using Application.Business.Positioning;
using Domain.Entities;
using Domain.Enums;
using TradeSimulator.Business;

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
            // Close first position
            var firstPosition = _openPositions.First();
            var positionUpdate = new CloseInstruction(
                        firstPosition, 1, DateTime.Now);
            positionUpdate.ClosePrice = 1;
            var positionHandler = new PositionHandler(
                new List<IPositionInstruction> {
                    positionUpdate }, 
                 ref _openPositions, ref _closedPositions);
            positionHandler.ExecuteInstructions();
            Assert.AreEqual(1, _openPositions.Count);
            Assert.AreEqual(3, _closedPositions.Count);

            // Close second position
            firstPosition = _openPositions.First();
            positionUpdate = new CloseInstruction(
                         firstPosition, 1, DateTime.Now);
            positionUpdate.ClosePrice = 2;
            positionHandler = new PositionHandler(
                new List<IPositionInstruction> {
                    positionUpdate }, 
                 ref _openPositions, ref _closedPositions);
            positionHandler.ExecuteInstructions();
            Assert.AreEqual(0, _openPositions.Count);
            Assert.AreEqual(4, _closedPositions.Count);
        }

        [TestMethod]
        public void OpenPositionTest()
        {
            var positionHandler = new PositionHandler(
               new List<IPositionInstruction> {
                    new OpenInstruction(
                        new Position
                        {
                            EntryPrice = 1,
                            StopLoss = 1,
                            Created = DateTime.Now,
                            Volume = 1,
                            Status = PositionStatus.OPEN,
                            SymbolName = "Test",
                            Commission = 0,
                            TakeProfit = 1,
                            PositionType = PositionType.BUY,
                            Margin = 0,
                            Comment = "Test",

                        }) },
               ref _openPositions, ref _closedPositions);
            positionHandler.ExecuteInstructions();
            Assert.AreEqual(3, _openPositions.Count);
            Assert.AreEqual(2, _closedPositions.Count);


            positionHandler = new PositionHandler(
                new List<IPositionInstruction> {
                    new OpenInstruction(
                        new Position
                        {
                            EntryPrice = 1,
                            StopLoss = 1,
                            Created = DateTime.Now,
                            Volume = 1,
                            Status = PositionStatus.OPEN,
                            SymbolName = "Test",
                            Commission = 0,
                            TakeProfit = 1,
                            PositionType = PositionType.BUY,
                            Margin = 0,
                            Comment = "Test",
                        }) },
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
            var positionUpdate = new ModifyInstruction(
                firstPosition,
                expectedStopLoss,
                expectedTakeProfit);
            var positionHandler = new PositionHandler(
               new List<IPositionInstruction> { positionUpdate },
                 ref _openPositions, ref _closedPositions);
            positionHandler.ExecuteInstructions();

            var modifiedPosition = _openPositions.First();
            Assert.AreEqual(expectedStopLoss, modifiedPosition.StopLoss);
            Assert.AreEqual(expectedTakeProfit, modifiedPosition.TakeProfit);
        }
    }
}
