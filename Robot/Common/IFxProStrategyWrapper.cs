namespace Robots.Common
{
    public interface IFxProStrategyWrapper
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
