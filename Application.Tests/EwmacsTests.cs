using System;
using NUnit.Framework;
using PikUpStix.Trading.Common.Indicator;

namespace PikUpStix.Trading.NTests
{
    [TestFixture]
    public class EwmacTests
    {
        [Test]
        public void GetEmacTest()
        {
            var ewmac = new Ewmac();
            Assert.AreEqual(Convert.ToDecimal(13.3725), ewmac.GetEwmac(Convert.ToDecimal(12.51),
                Convert.ToDecimal(0.25), Convert.ToDecimal(13.66)));
        }
    }
}