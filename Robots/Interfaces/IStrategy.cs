using Robots.Common;

namespace Robots.Interfaces
{
    public interface IStrategy
    {
        List<PositionUpdate> PositionInstructions { get; set; }
        List<string> LogMessages { get; set; }
    }
    public enum InstructionType
    {
        Open,
        Close,
        Modify,
        Leave,
        PlaceOrder,
        CancelOrder
    }
}
