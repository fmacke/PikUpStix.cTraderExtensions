using System;
using System.Linq;
using NUnit.Framework;
using PikUpStix.IBGateway;
using PikUpStix.Trading.Data.Local.SqlDb;

namespace PikUpStix.PythonBridge.NTests
{
    [TestFixture]
    public class GetMaximumAdverseExcursion
    {
        [Test]
        public void GetMaximumAdverseExcursionTest()
        {
            const int testId = 1059;
            var db = new TraderDbContext();
            var results = db.Results.Where(x => x.TestId == testId);
            foreach (var result in results)
            {

            }
        }
    }

    public class MaximumFavourableExcursion
    {
        public decimal Profit { get; set; }
        public decimal Drawdown { get; set; }
        public decimal ProfitPercent { get; set; }
        public decimal DrawdownPercent { get; set; }

        //public MaximumFavourableExcursion(decimal profit, decimal existingCapital, )
        //{
        //    Profit = profit;
        //    Drawdown = 
        //}}
    }
}