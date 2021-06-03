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
using Contact.Api.Common.Models;
using Moq;
using MongoDB.Bson;
using Contact.Api.Common.Helper;
using Contact.Api.Common.Dto;
using Newtonsoft.Json;

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

        #region Get_Contact

        [TestMethod()]
        public void Get_Contact_Without_Informations_From_Cache()
        {
            var uuid = GetNewGuid();
            var model = new ContactDetailDto
            {
                Firm = "test_firm",
                Lastname = "test_lastname",
                Name = "test_name",
                UUID = uuid
            };

            var json = JsonConvert.SerializeObject(model);
            var retvalArray = Encoding.UTF8.GetBytes(json);

            string cacheKey = $"{CacheKeyHelper.Contact}_{uuid}";

            var cacheServiceMock = services.GetMock<IDistributedCache>();
            cacheServiceMock.Setup(x => x.Get(cacheKey)).Returns(retvalArray);

            var result = contactService.GetContact(uuid);

            services.GetMock<IDistributedCache>().Verify(x => x.Get(cacheKey), Times.Exactly(1));

            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Verify(x => x.GetContact(uuid, Common.Enums.MongoCollectionType.Contact), Times.Never);

            Assert.AreEqual(result.Data.Firm, model.Firm);
            Assert.AreEqual(result.Data.Name, model.Name);
            Assert.AreEqual(result.Data.Lastname, model.Lastname);
            Assert.AreEqual(result.Data.UUID, model.UUID);
            Assert.AreEqual(result.Data.Informations.Count, 0);

            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 0);
        }

        [TestMethod()]
        public void Get_Contact_With_Informations_From_Cache()
        {
            var uuid = GetNewGuid();
            var model = new ContactDetailDto
            {
                Firm = "test_firm",
                Lastname = "test_lastname",
                Name = "test_name",
                UUID = uuid,
                Informations = new List<ContactInformationDto>
                {
                    new ContactInformationDto
                    {
                        ContactInformationType = Common.Enums.ContactInformationType.Email,
                        InformationDescription = "test_mail"
                    }
                }
            };

            var json = JsonConvert.SerializeObject(model);
            var retvalArray = Encoding.UTF8.GetBytes(json);

            string cacheKey = $"{CacheKeyHelper.Contact}_{uuid}";

            var cacheServiceMock = services.GetMock<IDistributedCache>();
            cacheServiceMock.Setup(x => x.Get(cacheKey)).Returns(retvalArray);

            var result = contactService.GetContact(uuid);

            services.GetMock<IDistributedCache>().Verify(x => x.Get(cacheKey), Times.Exactly(1));

            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Verify(x => x.GetContact(uuid, Common.Enums.MongoCollectionType.Contact), Times.Never);

            Assert.AreEqual(result.Data.Firm, model.Firm);
            Assert.AreEqual(result.Data.Name, model.Name);
            Assert.AreEqual(result.Data.Lastname, model.Lastname);
            Assert.AreEqual(result.Data.UUID, model.UUID);
            Assert.AreEqual(result.Data.Informations.Count, model.Informations.Count);

            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 0);
        }

        [TestMethod()]
        public void Get_Contact_Without_Informations_From_Mongo()
        {
            var uuid = GetNewGuid();
            var model = new ContactDetailDto
            {
                Firm = "test_firm",
                Lastname = "test_lastname",
                Name = "test_name",
                UUID = uuid
            };

            var json = JsonConvert.SerializeObject(model);
            var retvalArray = Encoding.UTF8.GetBytes(json);

            string cacheKey = $"{CacheKeyHelper.Contact}_{uuid}";

            var cacheServiceMock = services.GetMock<IDistributedCache>();
            cacheServiceMock.Setup(x => x.Get(cacheKey)).Returns((byte[])null);

            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Setup(x => x.GetContact(uuid, Common.Enums.MongoCollectionType.Contact)).Returns(new ContactEntityModel
            {
                Firm = model.Firm,
                Name = model.Name,
                Lastname = model.Lastname,
                UUID = model.UUID
            });

            var mapperBaseMock = services.GetMock<IMapperBase>().Setup(x => x.Map<ContactDetailDto>(It.IsAny<ContactEntityModel>())).Returns(new ContactDetailDto
            {
                Firm = model.Firm,
                Name = model.Name,
                Lastname = model.Lastname,
                UUID = model.UUID
            });

            mongoServiceMock.Setup(x => x.GetContactInformationList(uuid, Common.Enums.MongoCollectionType.Information))
                .Returns(new List<InformationEntityModel> { });

            var result = contactService.GetContact(uuid);

            services.GetMock<IDistributedCache>().Verify(x => x.Get(cacheKey), Times.Exactly(1));
            services.GetMock<IMongoService>().Verify(x => x.GetContact(uuid, Common.Enums.MongoCollectionType.Contact), Times.Exactly(1));
            services.GetMock<IMongoService>().Verify(x => x.GetContactInformationList(uuid, Common.Enums.MongoCollectionType.Information), Times.Exactly(1));

            Assert.AreEqual(result.Data.Firm, model.Firm);
            Assert.AreEqual(result.Data.Name, model.Name);
            Assert.AreEqual(result.Data.Lastname, model.Lastname);
            Assert.AreEqual(result.Data.UUID, model.UUID);
            Assert.AreEqual(result.Data.Informations.Count, 0);

            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 0);
        }

        [TestMethod()]
        public void Get_Contact_With_Informations_From_Mongo()
        {
            var uuid = GetNewGuid();
            var model = new ContactDetailDto
            {
                Firm = "test_firm",
                Lastname = "test_lastname",
                Name = "test_name",
                UUID = uuid,
                Informations = new List<ContactInformationDto>
                {
                    new ContactInformationDto
                    {
                        ContactInformationType = Common.Enums.ContactInformationType.Email,
                        InformationDescription = "test_mail"
                    }
                }
            };

            var json = JsonConvert.SerializeObject(model);
            var retvalArray = Encoding.UTF8.GetBytes(json);

            string cacheKey = $"{CacheKeyHelper.Contact}_{uuid}";

            var cacheServiceMock = services.GetMock<IDistributedCache>();
            cacheServiceMock.Setup(x => x.Get(cacheKey)).Returns((byte[])null);

            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Setup(x => x.GetContact(uuid, Common.Enums.MongoCollectionType.Contact)).Returns(new ContactEntityModel
            {
                Firm = model.Firm,
                Name = model.Name,
                Lastname = model.Lastname,
                UUID = model.UUID
            });

            var mapperBaseMock = services.GetMock<IMapperBase>();
            mapperBaseMock.Setup(x => x.Map<ContactDetailDto>(It.IsAny<ContactEntityModel>())).Returns(new ContactDetailDto
            {
                Firm = model.Firm,
                Name = model.Name,
                Lastname = model.Lastname,
                UUID = model.UUID
            });

            mongoServiceMock.Setup(x => x.GetContactInformationList(uuid, Common.Enums.MongoCollectionType.Information))
                .Returns(model.Informations.Select(x => new InformationEntityModel
                {
                    ContactInformationType = x.ContactInformationType,
                    InformationDescription = x.InformationDescription
                }).ToList());

            mapperBaseMock.Setup(x => x.Map<InformationEntityModel, ContactInformationDto>(It.IsAny<InformationEntityModel>())).Returns(new ContactInformationDto { ContactInformationType = model.Informations.Select(x => x.ContactInformationType).FirstOrDefault(), InformationDescription = model.Informations.Select(x => x.InformationDescription).FirstOrDefault() });

            var result = contactService.GetContact(uuid);

            services.GetMock<IDistributedCache>().Verify(x => x.Get(cacheKey), Times.Exactly(1));
            services.GetMock<IMapperBase>().Verify(x => x.Map<ContactDetailDto>(It.IsAny<ContactEntityModel>()), Times.Exactly(1));
            services.GetMock<IMapperBase>().Verify(x => x.Map<InformationEntityModel, ContactInformationDto>(It.IsAny<InformationEntityModel>()), Times.Exactly(1));
            services.GetMock<IMongoService>().Verify(x => x.GetContact(uuid, Common.Enums.MongoCollectionType.Contact), Times.Exactly(1));
            services.GetMock<IMongoService>().Verify(x => x.GetContactInformationList(uuid, Common.Enums.MongoCollectionType.Information), Times.Exactly(1));

            Assert.AreEqual(result.Data.Firm, model.Firm);
            Assert.AreEqual(result.Data.Name, model.Name);
            Assert.AreEqual(result.Data.Lastname, model.Lastname);
            Assert.AreEqual(result.Data.UUID, model.UUID);
            Assert.AreEqual(result.Data.Informations.Count, model.Informations.Count);

            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 0);
        }


        #endregion

        #region Add_Contact

        [TestMethod()]
        public void Add_Contact_Without_Information()
        {
            ContactModel contactModel = new ContactModel
            {
                Firm = "test_firm",
                Lastname = "test_lastname",
                Name = "test_name",
            };

            var uuid = GetNewGuid();

            var mapperBaseMock = services.GetMock<IMapperBase>();

            mapperBaseMock.Setup(x => x.Map<ContactEntityModel>(contactModel)).Returns(new ContactEntityModel { Firm = contactModel.Firm, Lastname = contactModel.Lastname, Name = contactModel.Name, UUID = uuid });

            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Setup(x => x.AddDocument(It.IsAny<BsonDocument>(), Common.Enums.MongoCollectionType.Contact)).Returns(true);

            var result = contactService.AddContact(contactModel);

            services.GetMock<IMapperBase>().Verify(x => x.Map<ContactEntityModel>(contactModel), Times.Exactly(1));
            services.GetMock<IMongoService>().Verify(x => x.AddDocument(It.IsAny<BsonDocument>(), Common.Enums.MongoCollectionType.Contact), Times.Exactly(1));
            Assert.IsTrue(result.Data);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 0);
        }

        [TestMethod()]
        public void Add_Contact_With_Single_Information()
        {
            ContactModel contactModel = new()
            {
                Firm = "test_firm",
                Lastname = "test_lastname",
                Name = "test_name",
                InformationModels = new List<InformationModel>
                {
                    new InformationModel
                    {
                        ContactInformationType = Common.Enums.ContactInformationType.Phone,
                        InformationDescription = "test_Number"
                    }
                }
            };

            var uuid = GetNewGuid();

            var mapperBaseMock = services.GetMock<IMapperBase>();

            mapperBaseMock.Setup(x => x.Map<ContactEntityModel>(contactModel)).Returns(new ContactEntityModel { Firm = contactModel.Firm, Lastname = contactModel.Lastname, Name = contactModel.Name, UUID = uuid });

            mapperBaseMock.Setup(x => x.Map<InformationModel, InformationEntityModel>(It.IsAny<InformationModel>())).Returns(new InformationEntityModel
            {
                ContactInformationType = Common.Enums.ContactInformationType.Phone,
                InformationDescription = "test_Number"
            });

            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Setup(x => x.AddDocument(It.IsAny<BsonDocument>(), Common.Enums.MongoCollectionType.Contact)).Returns(true);
            mongoServiceMock.Setup(x => x.AddDocument(It.IsAny<BsonDocument>(), Common.Enums.MongoCollectionType.Information)).Returns(true);

            var result = contactService.AddContact(contactModel);

            services.GetMock<IMapperBase>().Verify(x => x.Map<ContactEntityModel>(contactModel), Times.Exactly(1));
            services.GetMock<IMapperBase>().Verify(x => x.Map<InformationModel, InformationEntityModel>(It.IsAny<InformationModel>()), Times.Exactly(1));
            services.GetMock<IMongoService>().Verify(x => x.AddDocument(It.IsAny<BsonDocument>(), Common.Enums.MongoCollectionType.Contact), Times.Exactly(1));
            services.GetMock<IMongoService>().Verify(x => x.AddDocument(It.IsAny<BsonDocument>(), Common.Enums.MongoCollectionType.Information), Times.Exactly(1));

            Assert.IsTrue(result.Data);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 0);
        }

        [TestMethod()]
        public void Add_Contact_With_Multiple_Information()
        {
            ContactModel contactModel = new()
            {
                Firm = "test_firm",
                Lastname = "test_lastname",
                Name = "test_name",
                InformationModels = new List<InformationModel>
                {
                    new InformationModel
                    {
                        ContactInformationType = Common.Enums.ContactInformationType.Phone,
                        InformationDescription = "test_number"
                    },
                     new InformationModel
                    {
                        ContactInformationType = Common.Enums.ContactInformationType.Email,
                        InformationDescription = "test_mail"
                    },

                }
            };

            var uuid = GetNewGuid();

            var mapperBaseMock = services.GetMock<IMapperBase>();

            mapperBaseMock.Setup(x => x.Map<ContactEntityModel>(contactModel)).Returns(new ContactEntityModel { Firm = contactModel.Firm, Lastname = contactModel.Lastname, Name = contactModel.Name, UUID = uuid });

            mapperBaseMock.Setup(x => x.Map<InformationModel, InformationEntityModel>(It.IsAny<InformationModel>())).Returns(new InformationEntityModel
            {
                ContactInformationType = Common.Enums.ContactInformationType.Phone,
                InformationDescription = "test_Number"
            });

            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Setup(x => x.AddDocument(It.IsAny<BsonDocument>(), Common.Enums.MongoCollectionType.Contact)).Returns(true);
            mongoServiceMock.Setup(x => x.AddDocument(It.IsAny<BsonDocument>(), Common.Enums.MongoCollectionType.Information)).Returns(true);

            var result = contactService.AddContact(contactModel);

            services.GetMock<IMapperBase>().Verify(x => x.Map<ContactEntityModel>(contactModel), Times.Exactly(1));
            services.GetMock<IMapperBase>().Verify(x => x.Map<InformationModel, InformationEntityModel>(It.IsAny<InformationModel>()), Times.Exactly(contactModel.InformationModels.Count));
            services.GetMock<IMongoService>().Verify(x => x.AddDocument(It.IsAny<BsonDocument>(), Common.Enums.MongoCollectionType.Contact), Times.Exactly(1));
            services.GetMock<IMongoService>().Verify(x => x.AddDocument(It.IsAny<BsonDocument>(), Common.Enums.MongoCollectionType.Information), Times.Exactly(contactModel.InformationModels.Count));

            Assert.IsTrue(result.Data);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 0);
        }
        #endregion

        #region Get_Contact_List
        [TestMethod()]
        public void Get_Contact_List_From_Cache_No_Record()
        {
            string cacheKey = CacheKeyHelper.Contact_List;

            var cacheServiceMock = services.GetMock<IDistributedCache>();
            cacheServiceMock.Setup(x => x.Get(cacheKey)).Returns(new byte[] { 91, 93 });

            var result = contactService.GetContacts();

            services.GetMock<IDistributedCache>().Verify(x => x.Get(cacheKey), Times.Exactly(1));

            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Verify(x => x.GetContactList(Common.Enums.MongoCollectionType.Contact), Times.Never);

            Assert.AreEqual(result.Data.Count, 0);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 0);
        }

        [TestMethod()]
        public void Get_Contact_List_From_Mongo_No_Record()
        {
            string cacheKey = CacheKeyHelper.Contact_List;

            var cacheServiceMock = services.GetMock<IDistributedCache>();
            cacheServiceMock.Setup(x => x.Get(cacheKey)).Returns((byte[])null);

            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Setup(x => x.GetContactList(Common.Enums.MongoCollectionType.Contact)).Returns(new List<ContactEntityModel> { });

            var result = contactService.GetContacts();

            services.GetMock<IDistributedCache>().Verify(x => x.Get(cacheKey), Times.Exactly(1));

            mongoServiceMock.Verify(x => x.GetContactList(Common.Enums.MongoCollectionType.Contact), Times.Exactly(1));

            Assert.AreEqual(result.Data.Count, 0);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 0);
        }


        [TestMethod()]
        public void Get_Contact_List_From_Cache_With_Single_Record()
        {
            string cacheKey = CacheKeyHelper.Contact_List;

            List<ContactDto> model = new()
            {
                new ContactDto
                {
                    Firm = "test_firm",
                    Lastname = "test_lastname",
                    Name = "test_name",
                    UUID = GetNewGuid()
                }
            };

            var json = JsonConvert.SerializeObject(model);
            var retvalArray = Encoding.UTF8.GetBytes(json);


            var cacheServiceMock = services.GetMock<IDistributedCache>();
            cacheServiceMock.Setup(x => x.Get(cacheKey)).Returns(retvalArray);

            var result = contactService.GetContacts();

            services.GetMock<IDistributedCache>().Verify(x => x.Get(cacheKey), Times.Exactly(1));

            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Verify(x => x.GetContactList(Common.Enums.MongoCollectionType.Contact), Times.Never);

            Assert.AreEqual(result.Data.Count, 1);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 0);
        }

        [TestMethod()]
        public void Get_Contact_List_From_Cache_With_Multi_Record()
        {
            string cacheKey = CacheKeyHelper.Contact_List;

            List<ContactDto> model = new()
            {
                new ContactDto
                {
                    Firm = "test_firm",
                    Lastname = "test_lastname",
                    Name = "test_name",
                    UUID = GetNewGuid()
                },
                new ContactDto
                {
                    Firm = "test_firm_2",
                    Lastname = "test_lastname_2",
                    Name = "test_name_2",
                    UUID = GetNewGuid()
                }
            };

            var json = JsonConvert.SerializeObject(model);
            var retvalArray = Encoding.UTF8.GetBytes(json);


            var cacheServiceMock = services.GetMock<IDistributedCache>();
            cacheServiceMock.Setup(x => x.Get(cacheKey)).Returns(retvalArray);

            var result = contactService.GetContacts();

            services.GetMock<IDistributedCache>().Verify(x => x.Get(cacheKey), Times.Exactly(1));

            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Verify(x => x.GetContactList(Common.Enums.MongoCollectionType.Contact), Times.Never);

            Assert.AreEqual(result.Data.Count, model.Count);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 0);
        }

        [TestMethod()]
        public void Get_Contact_List_From_Mongo_With_Single_Record()
        {
            string cacheKey = CacheKeyHelper.Contact_List;

            List<ContactDto> model = new()
            {
                new ContactDto
                {
                    Firm = "test_firm",
                    Lastname = "test_lastname",
                    Name = "test_name",
                    UUID = GetNewGuid()
                }
            };

            var json = JsonConvert.SerializeObject(model);
            var retvalArray = Encoding.UTF8.GetBytes(json);

            var cacheServiceMock = services.GetMock<IDistributedCache>();
            cacheServiceMock.Setup(x => x.Get(cacheKey)).Returns((byte[])null);

            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Setup(x => x.GetContactList(Common.Enums.MongoCollectionType.Contact))
                .Returns(new List<ContactEntityModel>
                {
                    new ContactEntityModel
                    {
                        Firm = model.Select(x=>x.Firm).FirstOrDefault(),
                        Name = model.Select(x=>x.Name).FirstOrDefault(),
                        Lastname = model.Select(x=>x.Lastname).FirstOrDefault(),
                        UUID = model.Select(x=>x.UUID).FirstOrDefault(),
                    }
                });

            var mapperBaseMock = services.GetMock<IMapperBase>();
            mapperBaseMock.Setup(x => x.Map<ContactEntityModel, ContactDto>(It.IsAny<ContactEntityModel>())).Returns(new ContactDto
            {
                Firm = "test_firm",
                Lastname = "test_lastname",
                Name = "test_name",
                UUID = GetNewGuid()
            });

            var result = contactService.GetContacts();

            services.GetMock<IDistributedCache>().Verify(x => x.Get(cacheKey), Times.Exactly(1));

            mongoServiceMock.Verify(x => x.GetContactList(Common.Enums.MongoCollectionType.Contact), Times.Exactly(1));

            Assert.AreEqual(result.Data.Count, model.Count);
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 0);
        }

        #endregion

        #region Update_Contact

        [TestMethod()]
        public void Update_Contact_Fail()
        {
            bool retval = false;
            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Setup(x => x.UpdateContact(It.IsAny<ContactDto>(), Common.Enums.MongoCollectionType.Contact)).Returns(retval);

            var result = contactService.UpdateContact(It.IsAny<ContactDto>());

            services.GetMock<IMongoService>().Verify(x => x.UpdateContact(It.IsAny<ContactDto>(), Common.Enums.MongoCollectionType.Contact), Times.Exactly(1));

            Assert.AreEqual(result.Data, retval);

            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 1);
        }

        [TestMethod()]
        public void Update_Contact_Success()
        {
            bool retval = true;
            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Setup(x => x.UpdateContact(It.IsAny<ContactDto>(), Common.Enums.MongoCollectionType.Contact)).Returns(retval);

            var result = contactService.UpdateContact(It.IsAny<ContactDto>());

            services.GetMock<IMongoService>().Verify(x => x.UpdateContact(It.IsAny<ContactDto>(), Common.Enums.MongoCollectionType.Contact), Times.Exactly(1));

            Assert.AreEqual(result.Data, retval);

            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 0);
        }
        #endregion

        #region Delete_Contact

        [TestMethod()]
        public void Delete_Contact_Fail()
        {
            string uuid = GetNewGuid();
            bool retVal = false;

            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Setup(x => x.DeleteContact(uuid, Common.Enums.MongoCollectionType.Contact)).Returns(retVal);

            var result = contactService.DeleteContact(uuid);

            services.GetMock<IMongoService>().Verify(x => x.DeleteContact(uuid, Common.Enums.MongoCollectionType.Contact), Times.Exactly(1));
            services.GetMock<IMongoService>().Verify(x => x.DeleteContactInformation(uuid, Common.Enums.MongoCollectionType.Information), Times.Never);

            Assert.AreEqual(result.Data, retVal);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 1);
        }

        [TestMethod()]
        public void Delete_Contact_Success()
        {
            string uuid = GetNewGuid();
            bool retVal = true;

            var mongoServiceMock = services.GetMock<IMongoService>();
            mongoServiceMock.Setup(x => x.DeleteContact(uuid, Common.Enums.MongoCollectionType.Contact)).Returns(retVal);
            mongoServiceMock.Setup(x => x.DeleteContactInformation(uuid, Common.Enums.MongoCollectionType.Information)).Returns(retVal);

            var result = contactService.DeleteContact(uuid);

            services.GetMock<IMongoService>().Verify(x => x.DeleteContact(uuid, Common.Enums.MongoCollectionType.Contact), Times.Exactly(1));
            services.GetMock<IMongoService>().Verify(x => x.DeleteContactInformation(uuid, Common.Enums.MongoCollectionType.Information), Times.Exactly(1));

            Assert.AreEqual(result.Data, retVal);
            Assert.AreEqual(result.ExceptionMessageList.Count, 0);
            Assert.AreEqual(result.ResponseMessageList.Count, 0);
        }


        #endregion

        #region Helpers

        private string GetNewGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
        #endregion
    }
}