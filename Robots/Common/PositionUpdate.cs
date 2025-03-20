using Domain.Enums;
using Domain.Entities;

namespace Robots.Common
{
    public class PositionUpdate
    {
        public Position Position { get; set; }
        public InstructionType InstructionType { get; set; }
        public double? CloseAt { get; set; }
        public double? AdjustStopLossTo { get; set; }
        public double? AdjustTakeProfitTo { get; set; }
        public PositionUpdate(Position pos, InstructionType instruction)
        {
            Position = pos;
            InstructionType = instruction;
        }
    }
}
