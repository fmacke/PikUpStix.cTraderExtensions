using System;
using Domain.Entities;

namespace Application.Business.Forecasts
{
    public interface IForecastValue
    {
        DateTime DateTime { get; set; }
        double Forecast { get; set; }
        double AskingPrice { get; set; }
        double BiddingPrice { get; set; }
        double InstrumentBlock { get; set; }
        double ShortForecast { get; }
        double LongForecast { get; }
        double MediumForecast { get; }
        //Instrument Instrument { get; set; }
        double CalculateForecast();
    }
}