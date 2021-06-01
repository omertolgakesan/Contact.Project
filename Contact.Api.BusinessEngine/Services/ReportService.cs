using Contact.Api.Common;
using Contact.Api.Common.Conracts;
using Contact.Api.Common.Dto;
using Contact.Api.Common.Enums;
using Contact.Api.Common.Helper;
using Contact.Api.Common.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contact.Api.Services
{
    public class ReportService : IReportService
    {
        private readonly IMongoService mongoService;
        private readonly IMapperBase mapperBase;
        private readonly IDistributedCache cacheService;

        public ReportService(IMongoService iMongoService, IMapperBase iMapperBase, IDistributedCache distributedCache)
        {
            mongoService = iMongoService;
            mapperBase = iMapperBase;
            cacheService = distributedCache;
        }

        public BaseServiceResponseModel<ContactReportDto> GetLocationReport(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                return ResponseMessageHelper<ContactReportDto>.ResponseError(AppMessageConstants.EmptyParameter);
            }

            string cacheKey = $"{CacheKeyHelper.Location_Report}_{location}";
            var cacheData = cacheService.Get(cacheKey);
            if (cacheData == null)
            {
                List<string> contactInformations = mongoService.GetContactUuidListByLocation(location, MongoCollectionType.Information);
                List<ContactEntityModel> result = mongoService.GetContactListListByUuid(contactInformations, MongoCollectionType.Contact);
                var retVal = mapperBase.Map<ContactReportDto>(result);
                retVal.Location = location;

                var json = JsonConvert.SerializeObject(retVal);
                var retvalArray = Encoding.UTF8.GetBytes(json);

                var options = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromHours(1))
                        .SetAbsoluteExpiration(DateTime.Now.AddSeconds(60));
                cacheService.Set(cacheKey, retvalArray, options);

                return ResponseMessageHelper<ContactReportDto>.ResponseOk(retVal);
            }
            else
            {
                var json = Encoding.UTF8.GetString(cacheData);
                var retval = JsonConvert.DeserializeObject<ContactReportDto>(json);
                return ResponseMessageHelper<ContactReportDto>.ResponseOk(retval);
            }
        }
    }
}
