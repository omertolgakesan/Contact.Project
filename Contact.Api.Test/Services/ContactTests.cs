using Microsoft.VisualStudio.TestTools.UnitTesting;
using Contact.Api.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contact.Api.Common.Conracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Distributed;
using Contact.Api.Test;

namespace Contact.Api.Service.Services.Tests
{
    [TestClass()]
    public class ContactTests
    {
        IContactService contactService;
        IServiceProvider services;

        [TestInitialize]
        public void Init()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMock<IDistributedCache>();
            serviceCollection.AddMock<IMongoService>();
            serviceCollection.AddMock<IMapperBase>();
            serviceCollection.AddSingleton<IContactService, ContactService>();
            services = serviceCollection.BuildServiceProvider();
            contactService = services.GetService<IContactService>();
        }
        [TestMethod()]
        public void AddContactTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DeleteContactTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetContactsTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UpdateContactTest()
        {
            Assert.Fail();
        }
    }
}