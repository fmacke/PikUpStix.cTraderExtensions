using Domain.Entities;

namespace TradeSimulator
{
    public abstract class TradeSimulateBase
    {
        private List<HistoricalData> TestSet { get;  set; }
        public List<HistoricalData> CurrentBars { get; private set; } = new List<HistoricalData>();
        public HistoricalData CurrentBar { get; private set; }
        public HistoricalData LastBar { get; private set; }
        public double InitialCapital { get; private set; }
        public TradeSimulateBase(double initialcapital, List<HistoricalData> bars)
        {
            TestSet = bars.OrderBy(bar => bar.Date).ToList();
            InitialCapital = initialcapital;
        }
        public void Run()
        {
            OnStart();
            for (int x=0; x<TestSet.Count; x++)
            {
                CurrentBars.Add(TestSet[x]);
                CurrentBar = TestSet[x];
                if (x > 0)
                    LastBar = TestSet[x - 1];
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
