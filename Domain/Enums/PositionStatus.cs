using System.ComponentModel;

namespace Domain.Enums
{
    public enum PositionStatus
    {
        [Description("Open")]
        OPEN = 0,
        [Description("Closed")]
        CLOSED = 1,
        [Description("Order")]
        ORDER = 2
    }
}