using Robots.Common;

namespace Robots.Interfaces
{
    public interface IPositionInstructions
    {
        List<PositionUpdate> PositionInstructions { get; set; }
    }
    public enum InstructionType
    {
        Open,
        Close,
        Modify,
        Leave
    }
}
