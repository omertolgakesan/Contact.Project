using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contact.Api.Common.Helper
{
    public static class ApplicationMessageHelper
    {
        private static ApplicationResponse GetReponse(string key)
        {
            var _appSettings = DIServiceProvider.ServiceProvider.GetService<IOptions<AppSetting>>();
            var applicationResponses = _appSettings?.Value?.ApplicationResponses;
            if (!string.IsNullOrWhiteSpace(key) && applicationResponses?.Count > 0)
            {
                return applicationResponses.Find(x => x.Key == key) ?? applicationResponses.FirstOrDefault(x => x.Key == "GenericException");
            }

            return null;
        }

        public static ResponseMessageModel GetApplicationResponseMessage(string key)
        {
            var responseMessage = new ResponseMessageModel();

            var response = GetReponse(key);
            if (response != null)
            {
                responseMessage.Code = Convert.ToInt32(response.Code);
                responseMessage.Message = response.Message;
            }

            return responseMessage;
        }
    }
}
