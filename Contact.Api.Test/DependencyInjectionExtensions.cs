using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Api.Test
{
    public static class DependencyInjectionExtensions
    {
        public static Mock<T> AddMock<T>(this IServiceCollection services, bool callbase = false) where T : class
        {
            var mock = new Mock<T>();
            mock.CallBase = callbase;
            services.AddSingleton<Mock<T>>(mock);
            services.AddSingleton<T>(mock.Object);
            return mock;
        }

        public static Mock<T> GetMock<T>(this IServiceProvider serviceProvider) where T : class
        {
            return serviceProvider.GetService<Mock<T>>();
        }
    }
}
