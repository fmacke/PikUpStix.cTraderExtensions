namespace Application.Business.Calculations
{
    public class StandardDeviation : ICalculate
    {
        private double[] values;
        public StandardDeviation(double[] values) { 
            this.values = values ?? throw new ArgumentNullException(nameof(values));
        }
        public double Calculate()
        {
            double average = values.Average();
            double sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / values.Length);
            return sd;
        }
        //public static class StandardDeviation
        //{
        //    public static double Calculate(double[] values)
        //    {
        //        double average = values.Average();
        //        double sumOfSquaresOfDifferences = values.Select(val => (val - average) * (val - average)).Sum();
        //        double sd = Math.Sqrt(sumOfSquaresOfDifferences / values.Length);
        //        return sd;
        //    }
        //}
    }
}