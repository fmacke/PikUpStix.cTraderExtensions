using Application.Business.Indicator;
using Application.Business.Indicator.Signal;
using Application.Business.Market;
using Application.Business.Positioning.Handlers;
using Application.Business.Positioning.Instructions;
using Application.Business.Positioning.Validation;
using Domain.Entities;
using Domain.Enums;
using System.Collections.Generic;

namespace TradeSimulateTests
{
    [TestClass]
    public sealed class PositionHandlerTests
    {
        List<Position> _positions;
        IValidationService _validationService;
        List<IMarketInfo> _marketInfo;

        [TestInitialize]
        public void TestInitialize()
        {
            _validationService = new ValidationService();
            _marketInfo = new List<IMarketInfo>{
                new MarketInfo(DateTime.Now, 2,2,new List<Position>(),new List<HistoricalData>(),"Test", "GBP", 10000, 0.0001, 100000,1,
                    new ConfirmingSignals(new List<ISignal>()), Domain.Enums.TimeFrame.W1)
            };
            LoadPositions();
        }

        [TestMethod]
        public void ClosePositionTest()
        {
            // Close first position
            var firstPosition = _positions.First();
            var positionUpdate = new CloseInstruction(
                        firstPosition, 1, DateTime.Now, _validationService);
            positionUpdate.ClosePrice = 1;
            var positionHandler = new PositionHandler(
                new List<IPositionInstruction> {
                    positionUpdate }, 
                 ref _positions, _marketInfo);
            positionHandler.ExecuteInstructions();
            Assert.AreEqual(3, _positions.Where(p => p.ClosedAt == null).Count());
            Assert.AreEqual(1, _positions.Where(p => p.ClosedAt != null).Count());

            // Close second position
            firstPosition = _positions.Where(p => p.Status != PositionStatus.CLOSED).First();
            positionUpdate = new CloseInstruction(
                         firstPosition, 1, DateTime.Now, _validationService);
            positionUpdate.ClosePrice = 2;
            positionHandler = new PositionHandler(
                new List<IPositionInstruction> {
                    positionUpdate }, 
                 ref _positions, _marketInfo);
            positionHandler.ExecuteInstructions();
            Assert.AreEqual(2, _positions.Where(p => p.ClosedAt == null).Count());
            Assert.AreEqual(2, _positions.Where(p => p.ClosedAt != null).Count());
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

                        }, _validationService) },
               ref _positions, _marketInfo);
            positionHandler.ExecuteInstructions();
            Assert.AreEqual(5, _positions.Where(p => p.ClosedAt == null).Count());
            Assert.AreEqual(0, _positions.Where(p => p.ClosedAt != null).Count());


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
                        }, _validationService) },
                ref _positions, _marketInfo);
            positionHandler.ExecuteInstructions();
            Assert.AreEqual(6, _positions.Where(p => p.ClosedAt == null).Count());
            Assert.AreEqual(0, _positions.Where(p => p.ClosedAt != null).Count());
        }
        [TestMethod]
        public void ModifyPositionTest()
        {
            var expectedStopLoss = 2;
            var expectedTakeProfit = 3;
            var firstPosition = _positions.First();
            var positionUpdate = new ModifyInstruction(
                firstPosition,
                expectedStopLoss,
                expectedTakeProfit, _validationService);
            var positionHandler = new PositionHandler(
               new List<IPositionInstruction> { positionUpdate },
                 ref _positions, _marketInfo);
            positionHandler.ExecuteInstructions();

            var modifiedPosition = _positions.First();
            Assert.AreEqual(expectedStopLoss, modifiedPosition.StopLoss);
            Assert.AreEqual(expectedTakeProfit, modifiedPosition.TakeProfit);
        }
        private void LoadPositions()
        {
            _positions = new List<Position>
            {
                new Position()
                {
                    EntryPrice = 1,
                    StopLoss = 1,
                    TakeProfit = 1,
                    Volume = 1,
                    SymbolName = "Test",
                    Created = DateTime.Now
                },
                new Position()
                {
                    EntryPrice = 1,
                    StopLoss = 1,
                    TakeProfit = 1,
                    Volume = 1,
                    SymbolName = "Test",
                    Created = DateTime.Now
                },
                new Position()
                    {
                        EntryPrice = 1,
                        ClosePrice = 1,
                        StopLoss = 1,
                        TakeProfit = 1,
                        Volume = 1,
                        Created = DateTime.Now
                    },
                new Position()
                    {
                        EntryPrice = 1,
                        ClosePrice = 2,
                        StopLoss = 1,
                        TakeProfit = 1,
                        Volume = 1,
                        Created = DateTime.Now
                    }
            };
        }
    }
}
