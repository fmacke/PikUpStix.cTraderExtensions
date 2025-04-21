using Application.Business.Calculations;
using Domain.Entities;

namespace Application.Tests.Calculations
{
    [TestFixture]
    public class VolatilityTests
    {
        private double[] data =
        {
            1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,
            20,21,22,23,24,25,26
        };
        [OneTimeSetUp]
        public void Init()
        {
          
        }        

        [Test]
        public void VolatilityAsPercentage_Full_ArrayTest()
        {
            var priceVol = new VolatilityAsPercentage(data, data.Length).Calculate();
            Assert.AreEqual(20.23, Math.Round(priceVol, 2));
            
        }
        [Test]
        public void VolatilityAsPercentage_Partial_ArrayTest()
        {
            var priceVol = new VolatilityAsPercentage(data, 7).Calculate();
            Assert.AreEqual(0.34, Math.Round(priceVol, 2));
        }
    }
}
