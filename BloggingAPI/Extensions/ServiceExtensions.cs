using BloggingAPI.Data;
using BloggingAPI.Services.Implementations;
using BloggingAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace BloggingAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services,
            IConfiguration configuration) =>
            services.AddDbContext<ApplicationDbContext>(options =>
                                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        public static void ConfigureBloggingService(this IServiceCollection services) =>
            services.AddScoped<IBloggingService, BloggingService>();
    }
}

