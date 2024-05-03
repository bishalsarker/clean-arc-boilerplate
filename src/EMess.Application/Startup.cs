using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Net.NetworkInformation;
using System.Reflection;

namespace EMess.Application
{
    public static class Startup
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services.AddMediatR(
                cfg => cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly));
        }
    }
}
