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
        private readonly IQueueService queueService;

        public ReportService(IQueueService iQueueService)
        {
            queueService = iQueueService;
        }

        public BaseServiceResponseModel<bool> LocationReport(string location)
        {
            if (string.IsNullOrEmpty(location))
            {
                return ResponseMessageHelper<bool>.ResponseError(AppMessageConstants.EmptyParameter);
            }

            List<ContactReportProducerRequestModel> request = new()
            {
                new ContactReportProducerRequestModel()
                {
                    Location = location
                }
            };

            var retVal = queueService.LocationReport(request);
            return ResponseMessageHelper<bool>.ResponseOk(retVal);
        }
    }
}
