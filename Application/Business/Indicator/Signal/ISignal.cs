namespace Application.Business.Indicator.Signal
{
    public interface ISignal
    {
        public string Instrument { get; set; }
        public string Name { get; set; }
        public double Forecast { get; set; }
    }
}