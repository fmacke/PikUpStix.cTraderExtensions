using Application.Common.Interfaces;
using Application.Common.Interfaces.Shared;
using Application.Interfaces.CacheRepositories;
using Application.Interfaces.Repositories;
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
        public TestCalls Tests { get; private set; }
        public TestParameterCalls TestParameters { get; private set; }
        public InstrumentCalls Instruments { get; private set; }
        public TestTradeCalls TestTrades { get; private set; }
        public DataService()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddApplicationLayer();
            serviceCollection.AddTransient<ITestCacheRepository, TestCacheRepository>();
            serviceCollection.AddTransient<ITestService, TestService>();
            serviceCollection.AddTransient<IInstrumentService, InstrumentService>();
            serviceCollection.AddTransient<ITestParametersService, TestParametersService>();
            serviceCollection.AddTransient<ITestTradesService, TestTradesService>();
            //serviceCollection.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            serviceCollection.AddPersistenceContexts();
            serviceCollection.AddApplicationLayer();
            serviceCollection.AddRepositories();
            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddDistributedMemoryCache();
            serviceCollection.AddDbContext<ApplicationDbContext>();
            Tests = new TestCalls(serviceCollection.BuildServiceProvider());
            Instruments = new InstrumentCalls(serviceCollection.BuildServiceProvider());
            TestParameters = new TestParameterCalls(serviceCollection.BuildServiceProvider());
            TestTrades = new TestTradeCalls(serviceCollection.BuildServiceProvider());
        }
    }

    //internal class AuthenticatedUserService : IAuthenticatedUserService
    //{
    //    public string UserId { get; set; }
    //    public string Username { get; set; }
    //    public AuthenticatedUserService()
    //    {
    //        UserId = "1";
    //        Username = "admin";
    //    }
    //}
}
