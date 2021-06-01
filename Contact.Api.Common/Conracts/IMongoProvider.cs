using Contact.Api.Common.Dto;
using Contact.Api.Common.Enums;
using Contact.Api.Common.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Api.Common.Conracts
{
    public interface IMongoProvider
    {
        bool AddDocument(BsonDocument document, MongoCollectionType mongoCollectionType);
        bool DeleteContact(string UUID, MongoCollectionType collectionType);
        bool DeleteContactInformation(string uUID, MongoCollectionType collectionType);
        List<ContactEntityModel> GetContactList(MongoCollectionType collectionType);
        List<InformationEntityModel> GetContactInformationList(string uUID, MongoCollectionType collectionType);
        List<string> GetContactUuidListByLocation(string location, MongoCollectionType collectionType);
        List<ContactEntityModel> GetContactListListByUuid(List<string> contactInformations, MongoCollectionType collectionType);
        bool UpdateContact(ContactDto contactDto, MongoCollectionType collectionType);
    }
}
