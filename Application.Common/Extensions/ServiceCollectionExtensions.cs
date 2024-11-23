using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationCommonLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
