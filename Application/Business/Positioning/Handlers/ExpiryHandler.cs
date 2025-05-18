using Application.Business.Market;
using Domain.Entities;
using Domain.Enums;

namespace Application.Business.Positioning.Handlers
{
    public class ExpiryHandler
    {
        private List<Position> positions;
        private DateTime cursorDate;
        private List<IMarketInfo> marketInfos;

        public ExpiryHandler(DateTime cursorDate, ref List<Position> positions, List<IMarketInfo> marketInfos)
        {
            this.positions = positions; ;
            this.cursorDate = cursorDate;
            this.marketInfos = marketInfos;
        }
        public void CloseOutExpiredPositions()
        {
            foreach (var marketInfo in marketInfos)
            {
                var positionsToClose = positions
                    .Where(p => p.Status == PositionStatus.OPEN
                        && p.ExpirationDate.HasValue
                        && p.ExpirationDate.Value <= cursorDate
                        && p.SymbolName == marketInfo.SymbolName
                        && p.ClosedAt == null);

                var closeHandler = new ClosePositionHandler(ref positions);

                foreach (var position in positionsToClose)
                {
                    closeHandler.ClosePosition(
                        position,
                        position.StopLoss.GetValueOrDefault(),
                        Convert.ToDateTime(cursorDate),
                        marketInfo.PipSize,
                        marketInfo.ExchangeRate);
                }
            }
        }
    }
}