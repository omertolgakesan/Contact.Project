using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Contact.Api.Common
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection RegisterContactCoreServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<AppSetting>(Configuration);

            return services;
        }

        public static IApplicationBuilder UseLotteryCore(this IApplicationBuilder app)
        {
            return app;
        }
    }
}
