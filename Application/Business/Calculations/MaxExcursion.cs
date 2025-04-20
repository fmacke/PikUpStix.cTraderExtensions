using Domain.Entities;
using Domain.Enums;

namespace Application.Business.Calculations
{
    public static class MaxExcursion
    {
        private static double _maximumAdverseExcursion = 0;

        public static double Get(List<Position> positions, double lastLowPrice)
        {
            foreach (var position in positions.Where(p => p.Status == PositionStatus.OPEN))
            {
                double adverseExcursion = (position.EntryPrice - lastLowPrice) / position.EntryPrice * 100;
                if (position.PositionType == PositionType.SELL)
                {
                    adverseExcursion = (lastLowPrice - position.EntryPrice) / position.EntryPrice * 100;
                }
                if (adverseExcursion > _maximumAdverseExcursion)
                {
                    _maximumAdverseExcursion = adverseExcursion;
                }
            }
            return _maximumAdverseExcursion;
        }
    }
}
