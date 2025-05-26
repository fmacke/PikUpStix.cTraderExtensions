using Application.Business.Indicator;

namespace Application.Tests.Indicator
{
    [TestFixture]
    public class EwmacTests
    {
        [Test]
        public void GetEmacTest()
        {
            var ewmac = new Ewmac();
            Assert.AreEqual(13.3725, ewmac.GetEwmac(12.51, 0.25, 13.66));
        }
    }
}