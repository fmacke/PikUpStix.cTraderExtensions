using Application.Business.BackTest.Reports;
using Application.Features.Instruments.Commands.Create;
using Application.Features.Tests.Commands.Update;
using Application.Features.TestTrades.Commands.Create;
using Application.Mappings;
using AutoMapper;
using cAlgo.API;
using DataServices;
using Domain.Entities;
using FXProBridge.DataConversions;

namespace FXProBridge.Capture
{
    /// <summary>
    /// The robot copies Bar data from FXPro and saves it to db
    /// </summary>
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public abstract class BarDataCapture : Robot
    {
        public DataService DataService { get; set; }
        public List<HistoricalData> BarsData {get; set;}
        public BarDataCapture()
        {
            DataService = new DataService();
        }

        protected override void OnStop()
        {
            var instrument = new Instrument();
            instrument.Provider = "FXPro";
            instrument.InstrumentName = Symbol.Name;
            instrument.DataName = Symbol.Name;
            instrument.Currency = Symbol.Name;
            instrument.Frequency = Bars.TimeFrame.ToString();
            instrument.DataSource = "FXPro";
            instrument.Format = "Bar";
            instrument.Sort = "Ascending";
            instrument.ContractUnit = Symbol.PipSize;
            instrument.ContractUnitType = Symbol.Description;
            instrument.PriceQuotation = "Pips";
            instrument.MinimumPriceFluctuation = Symbol.PipValue;
            instrument.HistoricalDatas = BarConvert.ConvertBars(Bars);
            Capture(instrument);
        }
        public void Capture(Instrument instrument)
        {
            var instrumentExists = false;
            var instruments = DataService.InstrumentCaller.GetAllInstrumentsCachedAsync();
            if (instruments.Any(x => x.InstrumentName == instrument.InstrumentName 
                && x.DataSource == instrument.DataSource
                && x.Frequency == instrument.Frequency))
            {
                instrumentExists = true;
            }
            if (!instrumentExists)
            {
                var config = new MapperConfiguration(cfg => cfg.AddProfile<InstrumentProfile>());
                var mapper = config.CreateMapper();
                var addInstrumentCommand = mapper.Map<CreateInstrumentCommand>(instrument);
                DataService.InstrumentCaller.AddInstrument(addInstrumentCommand);
            }
        }
    }
}