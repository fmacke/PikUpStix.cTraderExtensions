using Application.Business;
using cAlgo.API;
using Domain.Enums;
using FXProBridge.Capture;
using Robots.Interfaces;

namespace FXProBridge.Common
{
    /// <summary>
    /// This class is responsible for managing the positions of the robot.
    /// </summary>
    public abstract class PositionManager : RobotTestWrapper, IPositionManager
    {
        public void ManagePositions(IStrategy x)
        { 
            foreach (var instruction in x.PositionInstructions)
            {
                TradeType tradeType = instruction.Position.PositionType == Domain.Enums.PositionType.BUY ? TradeType.Buy : tradeType = TradeType.Sell;
                
                var position = Positions.FirstOrDefault(p => p.Id == instruction.Position.Id);
                switch (instruction.InstructionType)
                {
                    case InstructionType.Close:
                        var result = ClosePosition(position);
                        if (!result.IsSuccessful)
                            Print("error : {0}", result.Error);
                        break;
                    case InstructionType.Modify:
                        var stop = instruction.Position.PositionType == Domain.Enums.PositionType.BUY ?
                            instruction.Position.EntryPrice - (instruction.Position.StopLoss * Symbol.PipSize) :
                            instruction.Position.EntryPrice + (instruction.Position.StopLoss * Symbol.PipSize);
                        position.ModifyStopLossPrice(stop);
                        if (!position.VolumeInUnits.Equals(Symbol.NormalizeVolumeInUnits(instruction.Position.Volume)))
                            ModifyPosition(position, Symbol.NormalizeVolumeInUnits(instruction.Position.Volume));                        
                        break;
                    case InstructionType.Open:
                        var normalise = Symbol.NormalizeVolumeInUnits(instruction.Position.Volume);
                        ExecuteMarketOrder(tradeType, SymbolName, normalise, x.GetType().Name, instruction.Position.StopLoss, 
                            instruction.Position.TakeProfit);
                        break;
                    case InstructionType.PlaceOrder:
                        //var sl = (instruction.Position.EntryPrice - instruction.Position.StopLoss) * Symbol.PipSize;
                        var placeOrderRes = PlaceLimitOrder(tradeType,
                            instruction.Position.SymbolName,
                            Symbol.NormalizeVolumeInUnits(instruction.Position.Volume),
                            Convert.ToDouble(instruction.Position.EntryPrice),
                            GetType().Name,
                            Convert.ToDouble(instruction.Position.StopLoss),
                            Convert.ToDouble(instruction.Position.TakeProfit),
                            ProtectionType.Relative, 
                            instruction.Position.ExpirationDate);
                        break;
                    case InstructionType.CancelOrder:
                        foreach (var order in PendingOrders) 
                            if(instruction.Position.Id == order.Id)
                                CancelPendingOrder(order); 
                        break;
                }
            }
            foreach (var message in x.LogMessages)
            {
                Print(message);
            }
        }
    }
}