using GiantSms.Net.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GiantSms.Net.Extensions
{
    public static class GiantSmsServiceExtensions
    {
        public static void AddGiantSms(this IServiceCollection services) 
        {
            services.AddHttpClient();

            services.AddScoped<IGiantSmsService, GiantSmsService>();

        }
    }
}
