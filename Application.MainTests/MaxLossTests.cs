using Application.Business.Calculations;

namespace PikUpStix.Trading.NTests
{
    [TestFixture]
    public class MaxLossTests
    {
        [Test]
        public void MaxLoss_Tests()
        {
            decimal capital = 10000;
            decimal currentPositionValue = 0;
            decimal maxLoss = Convert.ToDecimal(0.02);
            
            var mxc = new MaxLossCheck(capital, currentPositionValue, maxLoss);
            Assert.AreEqual(false, mxc.ClosePosition);

            // Don't close position if position value is positive
            currentPositionValue = 500;
            mxc = new MaxLossCheck(capital, currentPositionValue, maxLoss);
            Assert.AreEqual(false, mxc.ClosePosition);
            
            // Close position if position loss exceeds max loss percentage
            currentPositionValue = Convert.ToDecimal(-200.01);
            mxc = new MaxLossCheck(capital, currentPositionValue, maxLoss);
            Assert.AreEqual(true, mxc.ClosePosition);

            // Don't close position if position loss does not exceed max 
            currentPositionValue = Convert.ToDecimal(-199.99);
            mxc = new MaxLossCheck(capital, currentPositionValue, maxLoss);
            Assert.AreEqual(false, mxc.ClosePosition);
        }
    }


}