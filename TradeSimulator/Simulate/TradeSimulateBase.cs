using Application.Business.Market;
using Domain.Entities;
using Domain.Enums;
using Robots.Results;

namespace TradeSimulator.Simulate
{
    public abstract class TradeSimulateBase
    {
        public TestResultsCapture ResultsCapture { get; set; }
        private List<HistoricalData> TestSet { get;  set; }
        public double InitialCapital { get; private set; }
        public IMarketInfo CurrentMarketInfo { get; set; }

        public TradeSimulateBase(double initialcapital, IMarketInfo baseMarketInfo)
        {
            CurrentMarketInfo = baseMarketInfo;
            TestSet = CurrentMarketInfo.Bars.OrderBy(bar => bar.Date).ToList();
            CurrentMarketInfo.Bars = new List<HistoricalData>();
            CurrentMarketInfo.CursorDate = TestSet.First().Date;
            CurrentMarketInfo.CurrentCapital = initialcapital;
            InitialCapital = initialcapital;
        }
        public void Run()
        {
            OnStart();
            for (int x=0; x<TestSet.Count; x++)
            {
                LoadCurrentMarketData(x);
                OnTick();
                if (IsNewBar(CurrentMarketInfo.BarTimeFrame))
                    OnBar();
            }
            OnStop();
        }

        private bool IsNewBar(TimeFrame barTimeFrame)
        {
            throw new NotImplementedException();
        }

        private void LoadCurrentMarketData(int x)
        {
            CurrentMarketInfo.Bars.Add(TestSet[x]);
            CurrentMarketInfo.CursorDate = TestSet[x].Date;
            CurrentMarketInfo.CurrentBar = TestSet[x];
            CurrentMarketInfo.Ask = TestSet[x].OpenPrice;
            CurrentMarketInfo.Bid = TestSet[x].OpenPrice;
            if (x > 0)
                CurrentMarketInfo.LastBar = TestSet[x - 1];
        }

        protected internal virtual void OnTick()
        {
            throw new NotImplementedException();  // This method should be overridden in the derived class
        }
        protected internal virtual void OnBar()
        {
            throw new NotImplementedException();  // This method should be overridden in the derived class
        }
        protected internal virtual void OnStart()
        {
            throw new NotImplementedException();  // This method should be overridden in the derived class
        }
        protected internal virtual void OnStop()
        {
            throw new NotImplementedException();  // This method should be overridden in the derived class
        }
    }
}
