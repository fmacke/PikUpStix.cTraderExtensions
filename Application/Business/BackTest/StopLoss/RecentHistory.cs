using Domain.Entities;

namespace Application.Business.BackTest.StopLoss
{
    internal class RecentHistory
    {
        private List<List<HistoricalData>> historicalDataSets;
        private int instrumentId;
        private DateTime cursorDate;

        public RecentHistory(List<List<HistoricalData>> historicalDataSets, int instrumentId, DateTime cursorDate)
        {
            this.historicalDataSets = historicalDataSets;
            this.instrumentId = instrumentId;
            this.cursorDate = cursorDate;
        }
    }
}