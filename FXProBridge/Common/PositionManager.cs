using Application.Business.Positioning;
using cAlgo.API;
using Domain.Enums;
using FXProBridge.Capture;

namespace FXProBridge.Common
{
    public abstract class PositionManager : RobotTestWrapper
    {
        public void ManagePositions(List<IPositionInstruction> instructions)
        { 
            foreach (var instruction in instructions)
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
                        ExecuteMarketOrder(tradeType, SymbolName, normalise, instruction.GetType().Name, instruction.Position.StopLoss, 
                            instruction.Position.TakeProfit);
                        break;
                    case InstructionType.CancelOrder:
                        foreach (var order in PendingOrders) 
                            if(instruction.Position.Id == order.Id)
                                CancelPendingOrder(order); 
                        break;
                }
            }
        }
    }
}