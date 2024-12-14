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
        public Tests Tests { get; private set; }
        public TestParameters TestParameters { get; private set; }
        public Instruments Instruments { get; private set; }
        public DataService()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddApplicationLayer();
            serviceCollection.AddTransient<ITestCacheRepository, TestCacheRepository>();
            serviceCollection.AddTransient<ITestService, TestService>();
            serviceCollection.AddTransient<IInstrumentService, InstrumentService>();
            serviceCollection.AddTransient<ITestParametersService, TestParametersService>();
            serviceCollection.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            serviceCollection.AddPersistenceContexts();
            serviceCollection.AddApplicationLayer();
            serviceCollection.AddRepositories();
            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddDistributedMemoryCache();
            serviceCollection.AddDbContext<ApplicationDbContext>();
            Tests = new Tests(serviceCollection.BuildServiceProvider());
            Instruments = new Instruments(serviceCollection.BuildServiceProvider());
            TestParameters = new TestParameters(serviceCollection.BuildServiceProvider());
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
