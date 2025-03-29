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
            DataService.InstrumentCaller.AddOrUpdateInstrument(instrument);
        }
        
    }
}