using Contact.Api.Common;
using Contact.Api.Common.Conracts;
using Contact.Api.Common.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Api.Data.Mongo
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection RegisterMongoServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<MongoSetting>(Configuration.GetSection("MongoSettings"));

            DIServiceProvider.ServiceProvider = services.BuildServiceProvider();
            return services;
        }
    }
}
