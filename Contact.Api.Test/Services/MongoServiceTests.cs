using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contact.Api.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Contact.Api.Common.Conracts;
using Contact.Api.Data.Mongo;

namespace Contact.Api.Service.Services.Tests
{
    [TestClass()]
    public class MongoServiceTests
    {
        private IMongoProvider mongoProvider;

        [TestInitialize]
        public void Init()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IMongoProvider, MongoProvider>();
            var services = serviceCollection.BuildServiceProvider();
            mongoProvider = services.GetService<IMongoProvider>();
        }

        [TestMethod()]
        public void AddDocumentTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteContactTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteContactInformationTest()
        {
            Assert.Fail();
        }
    }
}