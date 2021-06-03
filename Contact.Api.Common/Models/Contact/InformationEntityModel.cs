using Contact.Api.Common.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Models
{
    [Serializable]
    public class InformationEntityModel
    {
        [BsonId]
        public ObjectId _id { get; set; }
        public string UUID { get; set; }
        public ContactInformationType ContactInformationType { get; set; }
        public string InformationDescription { get; set; }
    }
}
