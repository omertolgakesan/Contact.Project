using Contact.Api.Common;
using Contact.Api.Common.Conracts;
using Contact.Api.Common.Dto;
using Contact.Api.Common.Enums;
using Contact.Api.Common.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Api.Data.Mongo
{
    public class MongoProvider : IMongoProvider
    {
        static readonly object LockMongoInstance = new object();
        private IOptions<AppSetting> Options { get; set; }
        private IMongoDatabase _mongoDatabase { get; set; }
        private IMongoDatabase MongoDatabase
        {
            get
            {
                lock (LockMongoInstance)
                {
                    if (_mongoDatabase == null)
                    {
                        var MongoClient = new MongoClient(Options.Value.MongoSettings.ConnectionString);
                        _mongoDatabase = MongoClient.GetDatabase(Options.Value.MongoSettings.DatabaseName);
                    }
                }
                return _mongoDatabase;
            }
        }

        public MongoProvider(IOptions<AppSetting> options)
        {
            Options = options;
        }

        public bool AddDocument(BsonDocument document, MongoCollectionType collectionType)
        {
            var retVal = true;
            var collection = MongoDatabase.GetCollection<BsonDocument>(collectionType.ToString("g"));
            try
            {
                collection.InsertOne(document);
            }
            catch (Exception ex)
            {
                retVal = false;
            }
            return retVal;
        }

        public bool DeleteContact(string UUID, MongoCollectionType collectionType)
        {
            var collection = MongoDatabase.GetCollection<ContactEntityModel>(collectionType.ToString("g"));

            var res = collection.DeleteMany(x => x.UUID == UUID);
            if (res.DeletedCount > 0)
            {
                return true;
            }

            return false;
        }

        public bool DeleteContactInformation(string UUID, MongoCollectionType collectionType)
        {
            var collection = MongoDatabase.GetCollection<InformationEntityModel>(collectionType.ToString("g"));
            var documentCount = collection.Find(x => x.UUID == UUID).CountDocuments();

            var result = collection.DeleteMany(x => x.UUID == UUID);
            if (documentCount == result.DeletedCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<ContactEntityModel> GetContactList(MongoCollectionType collectionType)
        {
            var collection = MongoDatabase.GetCollection<ContactEntityModel>(collectionType.ToString("g"));
            return collection.AsQueryable().ToList();
        }

        public List<InformationEntityModel> GetContactInformationList(string uUID, MongoCollectionType collectionType)
        {
            var collection = MongoDatabase.GetCollection<InformationEntityModel>(collectionType.ToString("g"));
            return collection.Find(x => x.UUID == uUID).ToList();
        }

        public bool UpdateContact(ContactDto contactDto, MongoCollectionType collectionType)
        {
            var collection = MongoDatabase.GetCollection<ContactEntityModel>(collectionType.ToString("g"));
            var filter = Builders<ContactEntityModel>.Filter.Eq(x => x.UUID, contactDto.UUID);
            var update = Builders<ContactEntityModel>.Update
                .Set(x => x.Name, contactDto.Name)
                .Set(x => x.Lastname, contactDto.Lastname)
                .Set(x => x.Firm, contactDto.Firm);

            var result = collection.UpdateOne(filter, update);
            return true;
        }

        public ContactEntityModel GetContact(string uuid, MongoCollectionType collectionType)
        {
            var collection = MongoDatabase.GetCollection<ContactEntityModel>(collectionType.ToString("g"));
            return collection.AsQueryable().FirstOrDefault(x => x.UUID == uuid);
        }

        public bool AddDocuments(List<BsonDocument> documents, MongoCollectionType mongoCollectionType)
        {
            var retVal = true;
            var collection = MongoDatabase.GetCollection<BsonDocument>(mongoCollectionType.ToString("g"));
            try
            {
                collection.InsertMany(documents);
            }
            catch (Exception ex)
            {
                retVal = false;
            }
            return retVal;
        }

        public bool DeleteContactInformation(List<InformationEntityModel> contactInformationEntityModels, MongoCollectionType collectionType)
        {
            var retval = new List<bool>();
            var collection = MongoDatabase.GetCollection<InformationEntityModel>(collectionType.ToString("g"));
            foreach (var contactInformationEntityModel in contactInformationEntityModels)
            {
                var filter = Builders<InformationEntityModel>.Filter.Where(x =>
                x.UUID == contactInformationEntityModel.UUID &&
                x.ContactInformationType == contactInformationEntityModel.ContactInformationType &&
                x.InformationDescription == contactInformationEntityModel.InformationDescription);
                var result = collection.DeleteOne(filter);
                retval.Add(result.DeletedCount > 0);
            }

            return retval.TrueForAll(x => x);
        }
    }
}
