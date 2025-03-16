using Application.Features.HistoricalDatas.Commands.Create;
using Application.Features.Instruments.Commands.Create;
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
    //[Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.FullAccess)]
    public abstract class BarDataCapture : Robot
    {
        public DataService DataService { get; set; }
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
            instrument.Currency = Symbol.BaseAsset.Name;
            instrument.Frequency = Bars.TimeFrame.ToString();
            instrument.DataSource = "FXPro";
            instrument.Format = "Bar";
            instrument.Sort = "Ascending";
            instrument.ContractUnit = Symbol.PipSize;
            instrument.ContractUnitType = Symbol.Description;
            instrument.PriceQuotation = "Pips";
            instrument.MinimumPriceFluctuation = Symbol.PipSize;
            instrument.HistoricalDatas = BarConvert.ConvertBars(Bars);
            Capture(instrument);
        }
        public void Capture(Instrument instrument)
        {
            var existingInstrumentData = new Instrument();
            var instruments = DataService.InstrumentCaller.GetAllInstrumentsCachedAsync();
            
            var config = new MapperConfiguration(cfg => cfg.AddProfile<InstrumentProfile>());
            var mapper = config.CreateMapper(); 
            if (instruments.Any(x => x.InstrumentName == instrument.InstrumentName 
                && x.DataSource == instrument.DataSource
                && x.Frequency == instrument.Frequency))
            {
                existingInstrumentData = mapper.Map<Instrument>(instruments.First(x => x.InstrumentName == instrument.InstrumentName
                    && x.DataSource == instrument.DataSource
                    && x.Frequency == instrument.Frequency));
                AddAnyNewDataToDb(existingInstrumentData, instrument);
            }
            else
            {
                var addInstrumentCommand = mapper.Map<CreateInstrumentCommand>(instrument);
                DataService.InstrumentCaller.AddInstrument(addInstrumentCommand);
            }
        }

        private void AddAnyNewDataToDb(Instrument existingInstrumentData, Instrument instrument)
        {
            var rangeToAdd = new CreateHistoricalDataRangeCommand();
            foreach (var bar in instrument.HistoricalDatas)
            {
                if(!existingInstrumentData.HistoricalDatas.Any(x => x.Date.Value.Year == bar.Date.Value.Year
                    && x.Date.Value.Month == bar.Date.Value.Month
                    && x.Date.Value.Day == bar.Date.Value.Day
                    && x.Date.Value.Hour == bar.Date.Value.Hour
                    && x.Date.Value.Minute == bar.Date.Value.Minute
                    && x.Date.Value.Second == bar.Date.Value.Second))
                {
                    // Add this new data to db
                    rangeToAdd.Add(new CreateHistoricalDataCommand()
                    {
                        Date = bar.Date,
                        OpenPrice = bar.OpenPrice,
                        ClosePrice = bar.ClosePrice,
                        LowPrice = bar.LowPrice,
                        HighPrice = bar.HighPrice,
                        Volume = bar.Volume,
                        Settle = bar.Settle,
                        OpenInterest = bar.OpenInterest,
                        InstrumentId = instrument.Id
                    });
                }
            }
            DataService.HistoricalDataCaller.AddHistoricalDataRange(rangeToAdd);
        }
    }
}