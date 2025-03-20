using Robots.Common;

namespace Robots.Interfaces
{
    public interface IStrategy
    {
        List<PositionUpdate> GetPositionInstructions();
        List<string> LogMessages { get; set; }
    }
    
}
