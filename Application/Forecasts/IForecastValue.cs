using System;
using Domain.Entities;

namespace PikUpStix.Trading.Forecast
{
    public interface IForecastValue
    {
        DateTime DateTime { get; set; }
        Decimal Forecast { get; set; }
        double AskingPrice { get; set; }
        double BiddingPrice { get; set; }
        Decimal InstrumentBlock { get; set; }
        Decimal ShortForecast { get; }
        Decimal LongForecast { get; }
        Decimal MediumForecast { get; }
        Instrument Instrument { get; set; }
        decimal CalculateForecast();
    }
}