using Application;
using Application.Common.Interfaces;
using Application.Interfaces.CacheRepositories;
using Application.Interfaces.Repositories;
using Infrastructure.CacheRepositories;
using Infrastructure.Contexts;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPersistenceContexts(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        }
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddTransient<ITestRepository, TestRepository>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ITestCacheRepository, TestCacheRepository>();
            services.AddTransient<IInstrumentRepository, InstrumentRepository>();
            services.AddTransient<IInstrumentCacheRepository, InstrumentCacheRepository>();
            services.AddTransient<ITestParametersRepository, TestParametersRepository>();
            services.AddTransient<IPositionRepository, PositionRepository>();
            services.AddTransient<IPositionCacheRepository, PositionCacheRepository>();
            services.AddTransient<IHistoricalDataRepository, HistoricalDataRepository>();
            services.AddTransient<IHistoricalDataCacheRepository, HistoricalDataCacheRepository>();

        }
    }
}
