using Application.Business;
using FXProBridge.Capture;
using Robots.Interfaces;

namespace FXProBridge.Common
{
    /// <summary>
    /// This class is responsible for managing the positions of the robot.
    /// </summary>
    public abstract class PositionManager : RobotTestWrapper, IPositionManager
    {
        public void ManagePositions(IPositionInstructions x)
        {
            foreach (var instruction in x.PositionInstructions)
            {
                var tradeType = cAlgo.API.TradeType.Buy;
                if (instruction.InstructionType == InstructionType.Open)
                {

                    if (instruction.TradeType == Domain.Enums.TradeType.Sell)
                        tradeType = cAlgo.API.TradeType.Sell;
                }
                var position = Positions.FirstOrDefault(p => p.Id == instruction.Position.Id);
                switch (instruction.InstructionType)
                {
                    case InstructionType.Close:
                        var result = ClosePosition(position);
                        if (!result.IsSuccessful)
                            Print("error : {0}", result.Error);
                        break;
                    case InstructionType.Modify:
                        var stop = instruction.NewStopLoss;// * Symbol.PipSize;*// these need changed to be actual stop loss and take profit prices...
                        var tp = instruction.TakeProfit;// * Symbol.PipSize;
                        var res = ModifyPosition(position, stop, tp);
                        //var res = instruction.Position.ModifyStopLossPrice(stop);

                        var one = position.VolumeInUnits;
                        var two = Symbol.NormalizeVolumeInUnits(instruction.Volume);
                        if (!one.Equals(two))
                            res = ModifyPosition(position, Symbol.NormalizeVolumeInUnits(instruction.Volume));
                        //res = instruction.Position.ModifyVolume(Symbol.NormalizeVolumeInUnits(instruction.Volume));
                        break;
                    case InstructionType.Open:
                        var normalise = Symbol.NormalizeVolumeInUnits(instruction.Volume);
                        var res1 = ExecuteMarketOrder(tradeType, SymbolName, normalise, x.GetType().Name, instruction.NewStopLoss, instruction.TakeProfit);
                        break;
                }
            }
        }
    }
}