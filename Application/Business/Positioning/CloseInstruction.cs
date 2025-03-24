using Domain.Enums;
using Domain.Entities;

namespace Application.Business.Positioning
{
    public class CloseInstruction : PositionInstruction
    {
        public double ClosePrice { get; set; }
        public DateTime ClosedAt { get; set; }
        public CloseInstruction(Position pos, double closePrice, DateTime closedAt) : base(pos, InstructionType.Close)
        {
            CheckInstructionValid(closePrice, closedAt);
            ClosePrice = closePrice;
            ClosedAt = closedAt;
        }
        private static void CheckInstructionValid(double closePrice, DateTime closedAt)
        {
            if (closePrice <= 0)
            {
                throw new InvalidOperationException("Position close price must be greater than 0.");
            }
            if (closedAt == null)
            {
                throw new InvalidOperationException("Position closed date must be set.");
            }
        }
    }
}
