using Contact.Api.Common;
using Contact.Api.Common.Conracts;
using Contact.Api.Common.Dto;
using Contact.Api.Common.Enums;
using Contact.Api.Common.Helper;
using Contact.Api.Common.Models;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Api.Service.Services
{
    public class ContactService : IContactService
    {
        private readonly IDistributedCache cacheService;
        private readonly IMongoService mongoService;
        private readonly IMapperBase mapperBase;
        public ContactService(IMongoService iMongoService, IMapperBase iMapperBase, IDistributedCache distributedCache)
        {
            mongoService = iMongoService;
            mapperBase = iMapperBase;
            cacheService = distributedCache;
        }

        public BaseServiceResponseModel<ContactDetailDto> GetContact(string uuid)
        {
            string cacheKey = $"{CacheKeyHelper.Contact}_{uuid}";
            var cacheData = cacheService.Get(cacheKey);
            if (cacheData == null)
            {
                var contact = mongoService.GetContact(uuid, MongoCollectionType.Contact);
                if (contact == null)
                {
                    return ResponseMessageHelper<ContactDetailDto>.ResponseError(AppMessageConstants.RecordNotFound);
                }

                var retval = mapperBase.Map<ContactDetailDto>(contact);

                var contactInformationEntityList = mongoService.GetContactInformationList(uuid, MongoCollectionType.Information);

                var contactInformationDtoList = contactInformationEntityList.Select(mapperBase.Map<InformationEntityModel, ContactInformationDto>).ToList();

                retval.Informations = contactInformationDtoList;
                var json = JsonConvert.SerializeObject(retval);
                var retvalArray = Encoding.UTF8.GetBytes(json);

                var options = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromHours(1))
                        .SetAbsoluteExpiration(DateTime.Now.AddSeconds(60));
                cacheService.Set(cacheKey, retvalArray, options);

                return ResponseMessageHelper<ContactDetailDto>.ResponseOk(retval);
            }
            else
            {
                var json = Encoding.UTF8.GetString(cacheData);
                var retval = JsonConvert.DeserializeObject<ContactDetailDto>(json);
                return ResponseMessageHelper<ContactDetailDto>.ResponseOk(retval);
            }
        }

        public BaseServiceResponseModel<bool> AddContact(ContactModel contactModel)
        {
            var contactEntityModel = mapperBase.Map<ContactEntityModel>(contactModel);
            var contactResult = mongoService.AddDocument(contactEntityModel.ToBsonDocument(), MongoCollectionType.Contact);

            if (contactResult)
            {
                var informationEntityModelList = contactModel.InformationModels.Select(mapperBase.Map<InformationModel, InformationEntityModel>).ToList();

                List<bool> informationResulList = new();
                foreach (var informationEntityModel in informationEntityModelList)
                {
                    informationEntityModel.UUID = contactEntityModel.UUID;
                    var informationResult = mongoService.AddDocument(informationEntityModel.ToBsonDocument(), MongoCollectionType.Information);
                    informationResulList.Add(informationResult);
                }

                if (informationResulList.TrueForAll(x => x))
                {
                    return ResponseMessageHelper<bool>.ResponseOk(true);
                }
                else
                {
                    return ResponseMessageHelper<bool>.ResponseError(AppMessageConstants.GenericException);
                }
            }
            else
            {
                return ResponseMessageHelper<bool>.ResponseError(AppMessageConstants.GenericException);
            }

        }

        public BaseServiceResponseModel<bool> UpdateContact(ContactDto contactDto)
        {
            bool result = mongoService.UpdateContact(contactDto, MongoCollectionType.Contact);

            return ResponseMessageHelper<bool>.ResponseOk(result);
        }

        public BaseServiceResponseModel<bool> DeleteContact(string uuid)
        {
            bool retVal = mongoService.DeleteContact(uuid, Common.Enums.MongoCollectionType.Contact);
            if (retVal)
            {
                retVal = mongoService.DeleteContactInformation(uuid, Common.Enums.MongoCollectionType.Information);
                if (retVal)
                {
                    return ResponseMessageHelper<bool>.ResponseOk(true);
                }
                else
                {
                    return ResponseMessageHelper<bool>.ResponseError(AppMessageConstants.GenericException);
                }
            }
            else
            {
                return ResponseMessageHelper<bool>.ResponseOk(retVal, AppMessageConstants.RecordNotFound);
            }
        }

        public BaseServiceResponseModel<List<ContactDto>> GetContacts()
        {
            var cacheData = cacheService.Get(CacheKeyHelper.Contact_List);
            if (cacheData == null)
            {
                List<ContactEntityModel> contactList = mongoService.GetContactList(MongoCollectionType.Contact);
                var retval = contactList.Select(mapperBase.Map<ContactEntityModel, ContactDto>).ToList();

                var json = JsonConvert.SerializeObject(retval);
                var retvalArray = Encoding.UTF8.GetBytes(json);

                var options = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromHours(1))
                        .SetAbsoluteExpiration(DateTime.Now.AddSeconds(60));
                cacheService.Set(CacheKeyHelper.Contact_List, retvalArray, options);


                return ResponseMessageHelper<List<ContactDto>>.ResponseOk(retval);
            }
            else
            {
                var json = Encoding.UTF8.GetString(cacheData);
                var retval = JsonConvert.DeserializeObject<List<ContactDto>>(json);
                return ResponseMessageHelper<List<ContactDto>>.ResponseOk(retval);
            }
        }

        public BaseServiceResponseModel<bool> AddContactInformation(ContactInformationModel contactInformationModel)
        {
            var contact = mongoService.GetContact(contactInformationModel.UUID, MongoCollectionType.Contact);
            if (contact != null)
            {
                var contactInformationEntityModels = contactInformationModel.InformationModels.Select(mapperBase.Map<InformationModel, InformationEntityModel>).ToList();
                contactInformationEntityModels.ForEach(x => x.UUID = contactInformationModel.UUID);

                var documentList = contactInformationEntityModels.Select(x => x.ToBsonDocument()).ToList();

                var retVal = mongoService.AddDocuments(documentList, MongoCollectionType.Information);

                return ResponseMessageHelper<bool>.ResponseOk(retVal);
            }
            else
            {
                return ResponseMessageHelper<bool>.ResponseError(AppMessageConstants.RecordNotFound);
            }
        }

        public BaseServiceResponseModel<bool> DeleteContactInformation(ContactInformationModel contactInformationModel)
        {
            var contact = mongoService.GetContact(contactInformationModel.UUID, MongoCollectionType.Contact);
            if (contact != null)
            {
                var contactInformationEntityModels = contactInformationModel.InformationModels.Select(mapperBase.Map<InformationModel, InformationEntityModel>).ToList();
                contactInformationEntityModels.ForEach(x => x.UUID = contactInformationModel.UUID);
                var documentList = contactInformationEntityModels.Select(x => x.ToBsonDocument()).ToList();

                var retVal = mongoService.DeleteContactInformation(contactInformationEntityModels, MongoCollectionType.Information);
                if (retVal)
                {
                    return ResponseMessageHelper<bool>.ResponseOk(retVal);
                }
                else
                {
                    return ResponseMessageHelper<bool>.ResponseError(AppMessageConstants.RecordNotFound);
                }
            }
            else
            {
                return ResponseMessageHelper<bool>.ResponseError(AppMessageConstants.RecordNotFound);
            }
        }
    }
}
