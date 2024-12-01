using System;
using NUnit.Framework;
using PikUpStix.Trading.Data.Local.SqlDb;
using PikUpStix.Trading.BackTest.Reports;
using System.Linq;

namespace PikUpStix.Trading.NTests
{
    [TestFixture]
    public class TradeStatisticsTests
    {
        [Test]
        public void CalculateTestRunStatics()
        {
            var db = new TraderDBContextDerived();
            var tests = db.Tests.Where(x => x.TestId > 11722) ; 
             
            foreach (var test in tests)
            {
                var tradeStats = new TradeStatistics(test.TestId);
                try
                {
                    tradeStats.BuildReport(); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
