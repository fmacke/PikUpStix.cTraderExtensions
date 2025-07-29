using Application.Business.Market;
using Application.Business.Reports;
using Application.Business.Positioning.Handlers;
using Application.Interfaces;
using Domain.Entities;
using System.Diagnostics;
using Application.Common.Utilities;
using Application.Business.Positioning.Instructions;
using Robots.Results;
using DataServices;
using Application.Features.Positions.Commands.Create;
using Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace TradeSimulator.Simulate
{
    public class TradeSimulate : TradeSimulateBase
    {
        List<Position> Positions = new List<Position>();
        List<IPositionInstruction> PositionInstructions;
        public IStrategy Strategy { get; private set; }
        public bool SaveTestResult { get; }
        public DataService DataService { get; set; } = new DataService();

        public TradeSimulate(IMarketInfo runData, IStrategy strategy, double initialCapital, bool saveTestResult) : base(initialCapital, runData)
        {
            Strategy = strategy;
            SaveTestResult = saveTestResult;
        }
        protected internal override void OnTick()
        {
            Debug.WriteLine($"TradeSimulate OnTick() {CurrentMarketInfo.CursorDate}");
            List<IMarketInfo> marketInfos = GetMarketInfo();
            new ExpiryHandler(CurrentMarketInfo.CursorDate, ref Positions, marketInfos).CloseOutExpiredPositions();
            new StopLossHandler(CurrentMarketInfo.CursorDate, ref Positions, marketInfos).CloseOutStops();
            new TakeProfitHandler(CurrentMarketInfo.CursorDate, ref Positions, marketInfos).CloseOutTakeProfits();
        }
        protected internal override void OnBar()
        {
            Debug.WriteLine($"TradeSimulate OnBar() {CurrentMarketInfo.CursorDate}");
            List<IMarketInfo> marketInfos = GetMarketInfo();
            CurrentMarketInfo.CurrentCapital = Positions.Where(p => p.Status == PositionStatus.CLOSED).Sum(p => p.Margin) + InitialCapital;

            if (CurrentMarketInfo.CurrentCapital < 0)
                return;  // no money left.
            foreach (var mi in marketInfos)
                mi.CurrentCapital = CurrentMarketInfo.CurrentCapital;
            PositionInstructions = Strategy.CalculateChanges(marketInfos);
            new PositionHandler(PositionInstructions, ref Positions, marketInfos).ExecuteInstructions();
        }
        private List<IMarketInfo> GetMarketInfo()
        {
            List<IMarketInfo> marketInfos = new List<IMarketInfo>();
            CurrentMarketInfo.Positions = Positions;
            marketInfos.Add(CurrentMarketInfo);  // this works for now for testing purposes, where a strategy only deals with a single market instrument.            
            return marketInfos;
        }
        protected internal override void OnStart()
        {
            Debug.WriteLine($"TradeSimulate OnStart()");
            if (SaveTestResult)
                ResultsCapture = new TestResultsCapture("test begun at " + DateTime.Now.ToString(), InitialCapital, Strategy.TestParameters, DataService);
        }
        protected internal override void OnStop()
        {
            Debug.WriteLine("OnStop");
            var report = new TradeStatistics(Positions, InitialCapital, 20);
            if (SaveTestResult)
            {
                SaveTest(report);
            }
            Debug.WriteLine(ClassToString.FormatProperties(report));
        }

        private void SaveTest(TradeStatistics report)
        {
            if (ResultsCapture != null)
            {
                var tts = new CreatePositionRangeCommand();
                foreach (var tr in Positions)
                {
                    tts.Add(new CreatePositionCommand
                    {
                        TestId = ResultsCapture.TestId,
                        Comment = tr.Comment,
                        Created = tr.Created,
                        Volume = tr.Volume,
                        PositionType = tr.PositionType.ToString() == "BUY" ? PositionType.BUY : PositionType.SELL,
                        EntryPrice = tr.EntryPrice,
                        Commission = tr.Commission,
                        ClosedAt = tr.ClosedAt,
                        ClosePrice = tr.ClosePrice,
                        InstrumentId = tr.InstrumentId,
                        StopLoss = tr.StopLoss,
                        TakeProfit = tr.TakeProfit,
                        TrailingStop = tr.TrailingStop,
                        ExpirationDate = tr.ExpirationDate,
                        SymbolName = tr.SymbolName,
                        Status = PositionStatus.CLOSED,
                        Margin = tr.Margin
                    });
                }
                ResultsCapture.Capture("onStop", tts, DataService, 0);
                SaveReportToHtml();
            }
        }

        private void SaveReportToHtml()
        {
            var builder = new ConfigurationBuilder().AddUserSecrets<TradeSimulate>();
            var configuration = builder.Build();
            var saveResultTo = configuration["SaveTestResultsTo"] ?? throw new Exception("No directory set for test exports");
            string relativePath = @"..\net9.0\UI\TestVisualiser.py";
            string fullPath = Path.GetFullPath(relativePath);
            new PythonRunner().RunScript(fullPath,
                ResultsCapture.TestId + " 3 " + Strategy.GetType().Name + " " + DateTime.Now.ToShortDateString() + " " + saveResultTo);
        }
    }
}