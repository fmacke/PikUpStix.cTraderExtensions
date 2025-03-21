using Application.Business.Positioning;

namespace Application.Business.Strategy
{
    public interface IStrategy
    {
        List<IPositionInstruction> GetPositionInstructions();
        List<string> LogMessages { get; set; }
    }
    public interface IPositionManager
    {
        void ManagePositions(IStrategy x);
    }
}
