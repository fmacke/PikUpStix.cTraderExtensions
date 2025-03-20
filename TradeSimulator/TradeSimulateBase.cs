using Domain.Entities;

namespace TradeSimulator
{
    public abstract class TradeSimulateBase
    {
        public List<HistoricalData> Bars { get; private set; }
        public HistoricalData CurrentBar { get; private set; }
        public HistoricalData LastBar { get; private set; }
        public TradeSimulateBase(List<HistoricalData> bars)
        {
            Bars = bars.OrderBy(bar => bar.Date).ToList();
        }
        public void RunTradeSimulation()
        {
            OnStart();
            for (int x=0; x<Bars.Count; x++)
            {
                CurrentBar = Bars[x];
                LastBar = Bars[x - 1];
                OnBar();
            }
            OnStop();
        }
        protected internal virtual void OnBar()
        {
            // Do something
        }
        protected internal virtual void OnStart()
        {
            // Do something
        }
        protected internal virtual void OnStop()
        {
            // Do something
        }
    }
}
