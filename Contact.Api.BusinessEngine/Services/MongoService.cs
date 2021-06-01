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

        public bool DeleteContact(string UUID, MongoCollectionType collectionType)
        {
            return mongoProvider.DeleteContact(UUID, collectionType);
        }

        public bool DeleteContactInformation(string UUID, MongoCollectionType collectionType)
        {
            return mongoProvider.DeleteContactInformation(UUID, collectionType);
        }

        public List<InformationEntityModel> GetContactInformationList(string uUID, MongoCollectionType collectionType)
        {
            return mongoProvider.GetContactInformationList(uUID, collectionType);
        }

        public List<ContactEntityModel> GetContactList(MongoCollectionType collectionType)
        {
            return mongoProvider.GetContactList(collectionType);
        }

        public List<ContactEntityModel> GetContactListListByUuid(List<string> contactInformations, MongoCollectionType collectionType)
        {
            return mongoProvider.GetContactListListByUuid(contactInformations, collectionType);
        }

        public List<string> GetContactUuidListByLocation(string location, MongoCollectionType collectionType)
        {
            return mongoProvider.GetContactUuidListByLocation(location, collectionType);
        }

        public bool UpdateContact(ContactDto contactDto, MongoCollectionType collectionType)
        {
            return mongoProvider.UpdateContact(contactDto, collectionType);
        }
    }
}
