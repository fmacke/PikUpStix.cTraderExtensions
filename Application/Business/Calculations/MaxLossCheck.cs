namespace Application.Business.Calculations
{
    public class MaxLossCheck
    {
        public MaxLossCheck(decimal capital, decimal currentPositionValue, decimal maxLossPercent)
        {
            ClosePosition = false || currentPositionValue < 0 && currentPositionValue * -1 / capital > maxLossPercent;
        }

        public bool ClosePosition { get; set; }
    }
}