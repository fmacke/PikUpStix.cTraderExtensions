using cAlgo.API;
using Robots.Capture;
using Robots.Common;

namespace Robots
{
    /// <summary>
    /// All strategy injections should derive from this class.  Note this will log the test if inheritied from directly in cTrader.  However, a 
    /// IFxProStrategyWrapper object needs to be inserted into the OnBar method of a derived class in order for a trading strategy to actually run.
    /// </summary>
    /// 

    [Robot(AccessRights = AccessRights.FullAccess, AddIndicators = true)]
    public abstract class PikUpStixRobotRunner : RobotTestWrapper
    {
        public void ManagePositions(IFxProStrategyWrapper x)
        {
            foreach (var instruction in x.PositionInstructions)
            {
                switch (instruction.InstructionType)
                {
                    case InstructionType.Close:
                        var result = ClosePosition(instruction.Position);
                        if (!result.IsSuccessful)
                            Print("error : {0}", result.Error);
                        break;
                    case InstructionType.Modify:
                        var stop = instruction.NewStopLoss;// * Symbol.PipSize;*// these need changed to be actual stop loss and take profit prices...
                        var tp = instruction.TakeProfit;// * Symbol.PipSize;
                        //var res = ModifyPosition(instruction.Position, stop, tp);
                        var res = instruction.Position.ModifyStopLossPrice(stop);

                        var one = instruction.Position.VolumeInUnits;
                        var two = Symbol.NormalizeVolumeInUnits(instruction.Volume);
                        if (!one.Equals(two))
                            //res = ModifyPosition(instruction.Position, Symbol.NormalizeVolumeInUnits(instruction.Volume));                
                            res = instruction.Position.ModifyVolume(Symbol.NormalizeVolumeInUnits(instruction.Volume));
                        break;
                    case InstructionType.Open:
                        var normalise = Symbol.NormalizeVolumeInUnits(instruction.Volume);
                        var res1 = ExecuteMarketOrder(instruction.TradeType, SymbolName, normalise, x.GetType().Name, instruction.NewStopLoss, instruction.TakeProfit);
                        break;
                }
            }
        }
        protected override void OnStop()
        {
            if (IsTestRun)
                LogTestEnd(History);
        }
    }
}

















