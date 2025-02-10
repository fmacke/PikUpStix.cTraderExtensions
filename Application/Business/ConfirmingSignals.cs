using System.Linq;
using System.Collections.Generic;
using Application.Business.Indicator.Signal;

namespace Application.Business
{
    public class ConfirmingSignals : List<ISignal>
    {
        public double AggregatedForecast { get; set; } = 0.0;

        public ConfirmingSignals(List<ISignal> signals)
        {
            this.AddRange(signals);
            UpdateForecast();
        }

        public void AddSignal(ISignal signal)
        {
            this.Add(signal);
            UpdateForecast();
        }

        public void AddSignals(List<ISignal> signals)
        {
            this.AddRange(signals);
            UpdateForecast();
        }

        private void UpdateForecast()
        {
            if (this.Count > 0)
            {
                AggregatedForecast = this.Sum(x => x.Forecast) / this.Count;
            }
            else
            {
                AggregatedForecast = 0.0;
            }
        }
    }
}
