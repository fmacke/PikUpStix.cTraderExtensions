using Domain.Entities;

namespace TradeSimulator.Business
{
    public abstract class TradeSimulateBase
    {
        public List<HistoricalData> Bars { get; private set; }

        public TradeSimulateBase(List<HistoricalData> bars)
        {
            Bars = bars;
        }
        public void RunTradeSimulation()
        {
            OnStart();
            foreach (var bar in Bars)
                OnBar();
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
