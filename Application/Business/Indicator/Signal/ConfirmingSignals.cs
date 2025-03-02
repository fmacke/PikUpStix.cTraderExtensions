using System.Linq;
using System.Collections.Generic;

namespace Application.Business.Indicator.Signal
{
    public class ConfirmingSignals : List<ISignal>
    {
        public double AggregatedForecast { get; set; } = 0.0;

        public ConfirmingSignals(List<ISignal> signals)
        {
            AddRange(signals);
            UpdateForecast();
        }

        public void AddSignal(ISignal signal)
        {
            Add(signal);
            UpdateForecast();
        }

        public void AddSignals(List<ISignal> signals)
        {
            AddRange(signals);
            UpdateForecast();
        }

        private void UpdateForecast()
        {
            if (Count > 0)
            {
                AggregatedForecast = this.Sum(x => x.Forecast) / Count;
            }
            else
            {
                AggregatedForecast = 0.0;
            }
        }
    }
}
