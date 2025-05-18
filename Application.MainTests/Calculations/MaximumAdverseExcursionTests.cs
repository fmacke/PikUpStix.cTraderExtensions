using Application.Business.Calculations;
using Domain.Entities;

namespace Application.Tests.Calculations
{
    [TestFixture]
    public class MaximumAdverseExcursionTests
    {
        [Test]
        public void MaxDailyAdverseExcursionTest()
        {
            var results = new List<Position>()
            {
                new Position() { Margin = 100, ClosedAt = new DateTime(2000,1,1)},
                new Position() { Margin = -200, ClosedAt = new DateTime(2000,1,2) },
                new Position() { Margin = 300, ClosedAt = new DateTime(2000,1,3) },
                new Position() { Margin = -400, ClosedAt = new DateTime(2000,1,4) },
                new Position() { Margin = 600, ClosedAt = new DateTime(2000,1,5) },
                new Position() { Margin = -700, ClosedAt = new DateTime(2000,1,6) },
                new Position() { Margin = -800, ClosedAt = new DateTime(2000,1,7) },
                new Position() { Margin = 900, ClosedAt = new DateTime(2000,1,8) },
                new Position() { Margin = -1000, ClosedAt = new DateTime(2000,1,9) }
            };

            var mae = new DailyExcursions(results);

            Assert.AreEqual(400, Math.Round(mae.MaxFavourableExcursion, 2));  // Expected correct value
            Assert.AreEqual(-1200, Math.Round(mae.MaxAdverseExcursion, 2));   // Expected correct value
        }
    }
}
