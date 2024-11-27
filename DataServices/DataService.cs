using Application.Common.Interfaces;
using Application.Common.Interfaces.Shared;
using Application.Interfaces.CacheRepositories;
using Application.Services;
using DataServices.Calls;
using Infrastructure.CacheRepositories;
using Infrastructure.Contexts;
using Infrastructure.Extensions;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DataServices
{
    public class DataService
    {
        public Tests Tests { get; private set; }
        public Instruments Instruments { get; private set; }
        public DataService()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddApplicationLayer();
            serviceCollection.AddTransient<ITestCacheRepository, TestCacheRepository>();
            serviceCollection.AddTransient<ITestService, TestService>();
            serviceCollection.AddTransient<IInstrumentService, InstrumentService>();
            serviceCollection.AddScoped<IAuthenticatedUserService, AuthenticatedUserService>();
            serviceCollection.AddPersistenceContexts();
            serviceCollection.AddRepositories();
            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddDistributedMemoryCache();
            serviceCollection.AddDbContext<ApplicationDbContext>();
            Tests = new Tests(serviceCollection.BuildServiceProvider());
            Instruments = new Instruments(serviceCollection.BuildServiceProvider());
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
