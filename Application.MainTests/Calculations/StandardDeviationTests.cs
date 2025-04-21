using Application.Business.Calculations;
using Domain.Entities;

namespace Application.Tests.Calculations
{
    [TestFixture]
    public class StandardDeviationTests
    {
        double[] dummyData = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; 

        [OneTimeSetUp]
        public void Init()
        {
            
        }        

        [Test]
        public void StandardVolatility_Dynamic_Test()
        {

            ICalculate calculator = new StandardDeviation(dummyData);
            // Act
            double result = calculator.Calculate();
            // Assert
            double expectedStandardDeviation = CalculateStandardDeviation(dummyData);
            Assert.AreEqual(expectedStandardDeviation, result, 0.0001, "Standard deviation calculation is incorrect.");
        }
        [Test]
        public void StandardVolatility_Static_Test()
        {

            ICalculate calculator = new StandardDeviation(dummyData);
            // Act
            double result = calculator.Calculate();
            // Assert
            double expectedStandardDeviation = 2.8722813232690143;
            Assert.AreEqual(expectedStandardDeviation, result, 0.0001, "Standard deviation calculation is incorrect.");
        }

        private double CalculateStandardDeviation(double[] data)
        {
            double average = data.Average();
            double sumOfSquaresOfDifferences = data.Select(val => (val - average) * (val - average)).Sum();
            return Math.Sqrt(sumOfSquaresOfDifferences / data.Length);
        }
    }
}
