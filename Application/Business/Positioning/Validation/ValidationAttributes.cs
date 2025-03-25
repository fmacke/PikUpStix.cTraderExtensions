using Domain.Enums;
using Domain.Entities;

namespace Application.Business.Positioning.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidateCloseInstructionAttribute : Attribute
    {
        public void Validate(object value, Position pos)
        {
            if (value is double closePrice && closePrice <= 0)
            {
                throw new InvalidOperationException("Position close price must be greater than 0.");
            }
            if (value is DateTime closedAt && closedAt == null)
            {
                throw new InvalidOperationException("Position closed date must be set.");
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidateModifyInstructionAttribute : Attribute
    {
        public void Validate(object value, Position pos)
        {
            if (pos.Status != PositionStatus.OPEN)
            {
                throw new InvalidOperationException("Position must be open to modify.");
            }
            if (value == null)
            {
                throw new InvalidOperationException("At least one of STOP LOSS or TAKE PROFIT must be set.");
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidateOpenInstructionAttribute : Attribute
    {
        public void Validate(Position pos)
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
