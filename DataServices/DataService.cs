using Application.Common.Interfaces;
using Application.Common.Interfaces.Shared;
using Application.Interfaces.CacheRepositories;
using Application.Services;
using DataServices.Calls;
using Infrastructure.CacheRepositories;
using Infrastructure.Contexts;
using Infrastructure.Extensions;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataServices
{
    public class DataService : IDataService
    {
        public TestCalls TestCaller { get; private set; }
        public TestParameterCalls TestParameterCaller { get; private set; }
        public InstrumentCalls InstrumentCaller { get; private set; }
        public HistoricalDataCalls HistoricalDataCaller { get; private set; }
        public TestTradeCalls TestTradeCaller { get; private set; }
        public DataService()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddApplicationLayer();
            serviceCollection.AddTransient<ITestCacheRepository, TestCacheRepository>();
            serviceCollection.AddTransient<ITestService, TestService>();
            serviceCollection.AddTransient<IInstrumentService, InstrumentService>();
            serviceCollection.AddTransient<ITestParametersService, TestParametersService>();
            serviceCollection.AddTransient<ITestTradesService, TestTradesService>();
            serviceCollection.AddTransient<IHistoricalDataService, HistoricalDataService>();
            serviceCollection.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            serviceCollection.AddPersistenceContexts();
            serviceCollection.AddApplicationLayer();
            serviceCollection.AddRepositories();
            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddDistributedMemoryCache();
            serviceCollection.AddDbContext<ApplicationDbContext>();
            TestCaller = new TestCalls(serviceCollection.BuildServiceProvider());
            InstrumentCaller = new InstrumentCalls(serviceCollection.BuildServiceProvider());
            HistoricalDataCaller = new HistoricalDataCalls(serviceCollection.BuildServiceProvider());
            TestParameterCaller = new TestParameterCalls(serviceCollection.BuildServiceProvider());
            TestTradeCaller = new TestTradeCalls(serviceCollection.BuildServiceProvider());   
        }
    }

    internal class AuthenticatedUserService : IAuthenticatedUserService
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public AuthenticatedUserService()
        {
            UserId = "1";
            Username = "admin";
        }
    }
}
