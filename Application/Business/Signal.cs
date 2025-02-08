namespace Application.Business
{
    public class Signal : ISignal
    {
        public Signal(double value, string name)
        {
            Forecast = value;
            Name = name;
        }
        public string Name { get; set; }
        public string Instrument { get; set; }
        public double Forecast { get; set; }
    }
}
