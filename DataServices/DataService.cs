using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Features.Tests.Queries.GetAllCached;
using Application.Interfaces.CacheRepositories;
using Application.Services;
using DataServices.Calls;
using Infrastructure.CacheRepositories;
using Infrastructure.Contexts;
using Infrastructure.Extensions;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DataServices
{   
    public class DataService
    {
        public Tests Tests { get; private set; }
        public DataService()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddApplicationLayer();
            serviceCollection.AddTransient<ITestCacheRepository, TestCacheRepository>();
            serviceCollection.AddTransient<ITestService, TestService>();
            serviceCollection.AddPersistenceContexts();
            serviceCollection.AddRepositories();
            serviceCollection.AddTransient<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddDistributedMemoryCache();
            serviceCollection.AddDbContext<ApplicationDbContext>();
            Tests = new Tests(serviceCollection.BuildServiceProvider());
        }        
    }
}
