using Domain.Enums;
using Domain.Entities;

namespace Application.Business.Positioning
{
    public abstract class PositionInstruction : IPositionInstruction
    {
        public Position Position { get; set; }
        public InstructionType InstructionType { get; set; }
        public PositionInstruction(Position pos, InstructionType instruction)
        {
            Position = pos;
            InstructionType = instruction;
        }
    }
}
