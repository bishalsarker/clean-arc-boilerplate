using EMess.Application.Common.Interfaces;
using EMess.Application.Identity.Roles;
using EMess.Application.Identity.Tokens;
using EMess.Application.Identity.Users;
using EMess.Application.Interfaces.Persistence;
using EMess.Infrastructure.Auth;
using EMess.Infrastructure.Auth.Jwt;
using EMess.Infrastructure.Identity;
using EMess.Infrastructure.Permissions;
using EMess.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EMess.Infrastructure
{
    public static class Startup
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();

            // services.AddMediatR(o => Assembly.GetExecutingAssembly());

            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
            services.AddTransient<ApplicationDbInitializer>();
            services.AddTransient<ApplicationDbSeeder>();
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
        }

        public static async Task InitializeDatabasesAsync(this IServiceProvider services, CancellationToken cancellationToken = default)
        {
            using var scope = services.CreateScope();

            await scope.ServiceProvider
                .GetRequiredService<ApplicationDbInitializer>()
                .InitializeAsync(cancellationToken);
        }

        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration config)
        {
            return services
                .AddCurrentUser()
                .AddPermissions()
                .AddJwtAuth();
        }

        public static IApplicationBuilder UseCurrentUser(this IApplicationBuilder app) =>
            app.UseMiddleware<CurrentUserMiddleware>();

        private static IServiceCollection AddCurrentUser(this IServiceCollection services) =>
            services
                .AddScoped<CurrentUserMiddleware>()
                .AddScoped<ICurrentUser, CurrentUser>()
                .AddScoped(sp => (ICurrentUserInitializer)sp.GetRequiredService<ICurrentUser>());

        private static IServiceCollection AddPermissions(this IServiceCollection services) =>
            services
                .AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>()
                .AddScoped<IAuthorizationHandler, PermissionRequirementHandler>();

        public static IServiceCollection AddServices(this IServiceCollection services) =>
            services
                .AddScoped<IUserService, UserService>()
                .AddScoped<IRoleService, RoleService>()
                .AddScoped<ITokenService, TokenService>();
    }
}
