using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Application.Business.BackTest.Reports;


namespace PikUpStix.Trading.NTests
{
    [TestFixture]
    class AnnualReturnTests
    {
        [Test]
        public void AnnualReturn_Tests()
        {
            var results = LoadTestAnnualReturnData();
            var annualReturns = new AnnualReturns(1, results);
            Assert.Equals(annualReturns.First(x => x.Year.Year == 1980).ReturnAsPercentofInvestmentCapital, Convert.ToDecimal(0.3656343656343656343656343656M));
            Assert.Equals(annualReturns.First(x => x.Year.Year == 1981).ReturnAsPercentofInvestmentCapital, Convert.ToDecimal(0.3636363636363636363636363636M));
        }

        private List<Test_Trades> LoadTestAnnualReturnData()
        {
            var noOfDays = 730;
            var startDate = new DateTime(1980, 1, 1);
            var count = 0;
            var results = new List<Test_Trades>();
            //var cumulativeMargin = 1000;
            while (count < noOfDays)
            {
                //todo: this needs updated since Test_Results table was made redundant
                //results.Add(new Test_Trades() { Margin = 1, CumulativeMargin = cumulativeMargin + 1, CurrentDate = startDate });
                //startDate = startDate.AddDays(1);
                //count++;
            }
            return results;
        }
    }
}