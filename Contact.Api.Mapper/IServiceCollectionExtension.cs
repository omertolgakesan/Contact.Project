
using AutoMapper;
using Contact.Api.Common.Conracts;
using Contact.Api.Common.Helper;
using Contact.Api.Mapper.Contact;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Api.Mapper
{
    public static class IServiceCollectionExtension
    {
        public static IConfiguration _configuration { get; set; }

        private static void AddConfiguration()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            _configuration = builder.Build();
        }

        public static void RegisterMappingProfiles(this IServiceCollection services)
        {
            AddConfiguration();

            DIServiceProvider.ServiceProvider = services.BuildServiceProvider();

            var profiles = typeof(ContactMappingProfile)
                                    .Assembly
                                    .GetTypes()
                                    .Where(x => typeof(Profile)
                                                .IsAssignableFrom(x))
                                                .ToList();

            var mappingConfig = new MapperConfiguration(cfg =>
                                        cfg.AddMaps(profiles));

            IMapper mapper = mappingConfig.CreateMapper();

            services.AddSingleton(mapper);
            services.AddSingleton<IMapperBase, MapperBase>();

            mappingConfig.CompileMappings();
        }
    }
}
