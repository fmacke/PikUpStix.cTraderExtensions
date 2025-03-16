using DataServices;
using Application.Features.Tests.Commands.Create;
using Application.Features.Instruments.Commands.Create;
using Application.Features.HistoricalDatas.Commands.Create;
using Domain.Entities;
namespace Infrastructure.Tests
{

    [TestClass]
    public class DataServicesTests
    {
        public DataServicesTests()
        {
        }
        private DataService DataService { get; set; }

        [TestMethod]
        [Ignore("This test added entry to Test table")]
        public void AddTestToDB()
        {
            // Arrange
            DataService = new DataService();
            var testData = new CreateTestCommand()
            {
                FromDate = new DateTime(1900, 1, 1),
                ToDate = new DateTime(1900, 1, 1),
                StartingCapital = 0,
                EndingCapital = 0,
                Description = "CREATED BY UNIT TEST - AddTestToDB()",
                TestEndAt = DateTime.Now,
                TestRunAt = DateTime.Now
            };

            // Act
            var result = DataService.TestCaller.AddTest(testData); 

            // Assert
            //var okResult = result as OkObjectResult;
            Assert.IsNotNull(result);
            //Assert.IsInstanceOfType(result.Value, typeof(Result<List<GetAllInstrumentCachedResponse>>));
        }
        [TestMethod]
        [Ignore("This test adds entry to Instrument and HistoricalData table")]
        public void AddHistoricalDataToDB()
        {
            // Arrange
            DataService = new DataService();
            var instrument = new CreateInstrumentCommand()
            {
                ContractUnit = 1,
                Currency = "GPB",
                ContractUnitType = "GBP",
                DataSource = "FXPro",
                DataName = "GBP",
                Format = "Bar",
                Frequency = "1",
                InstrumentName = "AddHistoricalDataToDB - Test",
                MinimumPriceFluctuation = 1,
                PriceQuotation = "Pips",
                Provider = "FXPro",
                Sort = "Ascending"
            };
            for(int i = 0; i < 3; i++)
            {
                instrument.HistoricalDatas.Add(new HistoricalData()
                {
                    ClosePrice = i,
                    HighPrice = i,
                    LowPrice = i,
                    OpenPrice = i,
                    Date = DateTime.Now.AddDays(-i)
                });
            }

            // Act
            var instrumentResult = DataService.InstrumentCaller.AddInstrument(instrument);
           
            // Assert
            Assert.IsNotNull(instrumentResult);
        }
    
        [TestMethod]
        [Ignore("This test adds entry to Instrument and mulitiple entries to Instrument table")]
        public void AddHistoricalDataRangeToDB()
        {
            // Arrange
            DataService = new DataService();
            var instrument = new CreateInstrumentCommand()
            {
                ContractUnit = 1,
                Currency = "GPB",
                ContractUnitType = "GBP",
                DataSource = "FXPro",
                DataName = "GBP",
                Format = "Bar",
                Frequency = "1",
                InstrumentName = "AddHistoricalDataRangeToDB test",
                MinimumPriceFluctuation = 1,
                PriceQuotation = "Pips",
                Provider = "FXPro",
                Sort = "Ascending"
            };
            var instrumentResult = DataService.InstrumentCaller.AddInstrument(instrument);
            var historicalDatas = new CreateHistoricalDataRangeCommand();
            for (int i = 0; i < 3; i++)
            {
                historicalDatas.Add(new CreateHistoricalDataCommand()
                {
                    ClosePrice = i,
                    HighPrice = i,
                    LowPrice = i,
                    OpenPrice = i,
                    Date = DateTime.Now.AddDays(-i),
                    InstrumentId = instrumentResult
                });
            }

            // Act
            var hist = DataService.HistoricalDataCaller.AddHistoricalDataRange(historicalDatas);

            // Assert
            Assert.IsNotNull(hist);
        }
    }
}