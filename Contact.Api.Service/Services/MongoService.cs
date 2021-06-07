using Contact.Api.Common.Conracts;
using Contact.Api.Common.Dto;
using Contact.Api.Common.Enums;
using Contact.Api.Common.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Api.Service.Services
{
    public class MongoService : IMongoService
    {
        private readonly IMongoProvider mongoProvider;
        public MongoService(IMongoProvider iMongoProvider)
        {
            mongoProvider = iMongoProvider;
        }

        public bool AddDocument(BsonDocument document, MongoCollectionType mongoCollectionType)
        {
            return mongoProvider.AddDocument(document, mongoCollectionType);
        }

        public bool AddDocuments(List<BsonDocument> documents, MongoCollectionType collectionType)
        {
            return mongoProvider.AddDocuments(documents, collectionType);
        }

        public bool DeleteContact(string UUID, MongoCollectionType collectionType)
        {
            return mongoProvider.DeleteContact(UUID, collectionType);
        }

        public bool DeleteContactInformation(string UUID, MongoCollectionType collectionType)
        {
            return mongoProvider.DeleteContactInformation(UUID, collectionType);
        }

        public bool DeleteContactInformation(List<InformationEntityModel> contactInformationEntityModels, MongoCollectionType collectionType)
        {
            return mongoProvider.DeleteContactInformation(contactInformationEntityModels, collectionType);
        }

        public ContactEntityModel GetContact(string uuid, MongoCollectionType collectionType)
        {
            return mongoProvider.GetContact(uuid, collectionType);
        }

        public List<InformationEntityModel> GetContactInformationList(string uUID, MongoCollectionType collectionType)
        {
            return mongoProvider.GetContactInformationList(uUID, collectionType);
        }

        public List<ContactEntityModel> GetContactList(MongoCollectionType collectionType)
        {
            return mongoProvider.GetContactList(collectionType);
        }

        public bool UpdateContact(ContactDto contactDto, MongoCollectionType collectionType)
        {
            return mongoProvider.UpdateContact(contactDto, collectionType);
        }
    }
}
