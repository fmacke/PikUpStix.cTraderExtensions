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
    public class OpenInstruction : PositionInstruction
    {
        public OpenInstruction(Position pos) : base(pos, InstructionType.Open)
        {
            CheckInstructionIsValid(pos);
        }

        private static void CheckInstructionIsValid(Position pos)
        {
            if (pos.Status != PositionStatus.OPEN)
            {
                throw new InvalidOperationException("Position must be open to add.");
            }
            if (pos.Volume <= 0)
            {
                throw new InvalidOperationException("Position volume must be greater than 0.");
            }
            if (pos.EntryPrice <= 0)
            {
                throw new InvalidOperationException("Position entry price must be greater than 0.");
            }
            if (pos.Created == null)
            {
                throw new InvalidOperationException("Position created date must be set.");
            }
            if (pos.SymbolName == null)
            {
                throw new InvalidOperationException("Position symbol name must be set.");
            }
            if (pos.Commission < 0)
            {
                throw new InvalidOperationException("Position commission must be greater than or equal to 0.");
            }
            if (pos.StopLoss < 0)
            {
                throw new InvalidOperationException("Position stop loss must be greater than or equal to 0.");
            }
            if (pos.TakeProfit < 0)
            {
                throw new InvalidOperationException("Position take profit must be greater than or equal to 0.");
            }
            if (pos.TrailingStop < 0)
            {
                throw new InvalidOperationException("Position trailing stop must be greater than or equal to 0.");
            }
            if (pos.Status != PositionStatus.OPEN)
            {
                throw new InvalidOperationException("Position status must be open.");
            }
        }
    }
}
