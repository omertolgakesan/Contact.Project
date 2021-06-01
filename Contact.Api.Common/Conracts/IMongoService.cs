﻿using Contact.Api.Common.Dto;
using Contact.Api.Common.Enums;
using Contact.Api.Common.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Api.Common.Conracts
{
    public interface IMongoService
    {
        bool AddDocument(BsonDocument document, MongoCollectionType collectionType);
        bool DeleteContact(string UUID, MongoCollectionType collectionType);
        bool DeleteContactInformation(string UUID, MongoCollectionType ınformation);
        List<ContactEntityModel> GetContactList(MongoCollectionType collectionType);
        List<InformationEntityModel> GetContactInformationList(string uUID, MongoCollectionType collectionType);
        List<string> GetContactUuidListByLocation(string location, MongoCollectionType ınformation);
        List<ContactEntityModel> GetContactListListByUuid(List<string> contactInformations, MongoCollectionType contact);
        bool UpdateContact(ContactDto contactDto, MongoCollectionType contact);
    }
}
