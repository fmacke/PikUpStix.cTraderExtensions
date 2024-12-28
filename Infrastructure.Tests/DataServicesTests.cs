using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using AutoMapper;
using Application.Interfaces.CacheRepositories;
using Infrastructure.CacheRepositories;
using Application.Features.Tests.Queries.GetAllPaged;
using Application.Common.Results;
using System.Data.Entity.Core.Objects;
using Application.Features.Tests.Queries.GetAllCached;
using DataServices;
using Application.Features.Tests.Commands.Create;
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
                Description = "TEST",
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
    }
}