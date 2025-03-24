using Domain.Enums;
using Domain.Entities;

namespace Application.Business.Positioning
{
    public class ModifyInstruction : PositionInstruction
    {
        public double? AdjustStopLossTo { get; }
        public double? AdjustTakeProfitTo { get; }
        public ModifyInstruction(Position pos, double? adjustStopLossTo, double? adjustTakeProfitTo) : base(pos, InstructionType.Modify)
        {
            CheckInstructionValid(pos, adjustStopLossTo, adjustTakeProfitTo);
            AdjustStopLossTo = adjustStopLossTo;
            AdjustTakeProfitTo = adjustTakeProfitTo;
        } 
        private static void CheckInstructionValid(Position pos, double? adjustStopLossTo, double? adjustTakeProfitTo)
        {
            if (pos.Status != PositionStatus.OPEN)
            {
                throw new InvalidOperationException("Position must be open to modify.");
            }
            if (adjustStopLossTo == null && adjustTakeProfitTo == null)
            {
                throw new InvalidOperationException("At least one of STOP LOSS or TAKE PROFIT must be set.");
            }
        }
    }
}
