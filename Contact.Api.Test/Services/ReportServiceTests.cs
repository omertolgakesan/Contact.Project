using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contact.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Contact.Api.Common.Conracts;
using Contact.Api.Service.Services;
using Microsoft.Extensions.Caching.Distributed;
using Contact.Api.Test;

namespace Contact.Api.Services.Tests
{
    [TestClass()]
    public class ReportServiceTests
    {
        private IReportService reportService;
        [TestInitialize]
        public void Init()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMock<IMongoService>();
            serviceCollection.AddMock<IDistributedCache>();
            serviceCollection.AddMock<IMapperBase>();
            serviceCollection.AddSingleton<IReportService, ReportService>();
            var services = serviceCollection.BuildServiceProvider();
            reportService = services.GetService<IReportService>();
        }

        [TestMethod()]
        public void GetLocationReportTest()
        {
            Assert.Fail();
        }
    }
}