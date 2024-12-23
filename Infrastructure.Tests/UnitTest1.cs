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
namespace Infrastructure.Tests
{

    [TestClass]
    public class InstrumentControllerTests
    {
        public InstrumentControllerTests()
        {
            ConfigureServices();
        }
        private IServiceCollection services { get; set; }
        private readonly ITestService _testService;

        [TestInitialize]
        public void Setup(ITestService testService)
        {
            //_testService = new TestService(testService);

            //serviceProviderMock.Setup(sp => sp.GetService(typeof(IMediator))).Returns(_mediatorMock.Object);
            //_httpContext.RequestServices = serviceProviderMock.Object;

        }

        public void ConfigureServices()
        {
            // Register MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(InstrumentControllerTests).Assembly));

            // Register AutoMapper
            services.AddAutoMapper(typeof(InstrumentControllerTests));

            // Register your cache repository
            services.AddScoped<ITestCacheRepository, TestCacheRepository>();
            services.AddScoped<ITestService, TestService>();
            // Register other services
            //services.AddControllers();
        }


        [TestMethod]
        public async Task GetAllCached_ReturnsOkResult()
        {
            // Arrange
            var result = await _testService.GetAllTestsCachedAsync();

            // Act
            //var result = await _controller.GetAllCached();

            // Assert
            //var okResult = result as OkObjectResult;
            Assert.IsNotNull(result);
            //Assert.IsInstanceOfType(result.Value, typeof(Result<List<GetAllInstrumentCachedResponse>>));
        }
    }
}