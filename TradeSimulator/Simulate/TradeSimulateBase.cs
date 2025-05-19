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
                var lastDateTime = CurrentMarketInfo.CursorDate;
                LoadCurrentMarketData(x);
                var currentDateTime = CurrentMarketInfo.CursorDate; 
                OnTick();
                if (IsNewBar(CurrentMarketInfo.BarTimeFrame, currentDateTime, lastDateTime))
                    OnBar();
            }
            OnStop();
        }
        private bool IsNewBar(TimeFrame barTimeFrame, DateTime current, DateTime last)
        {
            if (barTimeFrame == TimeFrame.D1)
            { 
                if (current.Date.Day != last.Date.Day)
                    return true;
                return false;
            }
            if (barTimeFrame == TimeFrame.H1)
            {
                if (current.Hour != last.Hour)
                    return true;
                return false;
            }
            if (barTimeFrame == TimeFrame.M1)
            {
                if (current.Minute != last.Minute)
                    return true;
                return false;
            }
            if (barTimeFrame == TimeFrame.M5)
            {
                if (current.Minute % 5 != last.Minute % 5)
                    return true;
                return false;
            }
            if (barTimeFrame == TimeFrame.M15)
            {
                if (current.Minute % 15 != last.Minute % 15)
                    return true;
                return false;
            }
            if (barTimeFrame == TimeFrame.M30)
            {
                if (current.Minute % 30 != last.Minute % 30)
                    return true;
                return false;
            }
            if (barTimeFrame == TimeFrame.H4)
            {
                if (current.Hour % 4 != last.Hour % 4)
                    return true;
                return false;
            }
            if (barTimeFrame == TimeFrame.W1)
            {
                if (current.Date.DayOfWeek != last.Date.DayOfWeek)
                    return true;
                return false;
            }
            if (barTimeFrame == TimeFrame.MN1)
            {
                if (current.Date.Month != last.Date.Month)
                    return true;
                return false;
            }

            throw new NotImplementedException("TimeFrame not implemented yet");
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
