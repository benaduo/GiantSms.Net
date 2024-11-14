using GiantSms.Net.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiantSms.Net.Extensions
{
    public static class GiantSmsServiceExtensions
    {
        public static void AddGiantSms(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.Configure<GiantSmsConnection>(options =>
                configuration.GetSection(nameof(GiantSmsConnection)).Bind(options));
            services.AddScoped<IGiantSmsService, GiantSmsService>();
        }
    }
}