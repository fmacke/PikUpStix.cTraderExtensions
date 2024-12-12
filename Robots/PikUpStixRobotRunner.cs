using Robots.Capture;
using Robots.Common;

namespace Robots
{
    public class PikUpStixRobotRunner : RobotTestWrapper, IPikUpStixRobotRunner
    {
        protected override void OnStart()
        {
            var result = System.Diagnostics.Debugger.Launch();
            if (!result)
            {
                Print("Debugger launch failed");
            }
            base.OnStart();
        }
        public void ManagePositions(IFxProStrategyWrapper x)
        {
            foreach (var instruction in x.PositionInstructions)
            {
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
                        var res1 = ExecuteMarketOrder(position.TradeType, SymbolName, normalise, x.GetType().Name, instruction.NewStopLoss, instruction.TakeProfit);
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

















