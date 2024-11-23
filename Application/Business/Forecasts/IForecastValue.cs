using System;
using Domain.Entities;

namespace Application.Business.Forecasts
{
    public interface IForecastValue
    {
        DateTime DateTime { get; set; }
        decimal Forecast { get; set; }
        double AskingPrice { get; set; }
        double BiddingPrice { get; set; }
        decimal InstrumentBlock { get; set; }
        decimal ShortForecast { get; }
        decimal LongForecast { get; }
        decimal MediumForecast { get; }
        Instrument Instrument { get; set; }
        decimal CalculateForecast();
    }
}