using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Contact.Api.Common.Helper
{
    public class ResponseHelper<T> where T : new()
    {
        private readonly BaseServiceResponseModel<T> response;

        public ResponseHelper()
        {
            this.response = new BaseServiceResponseModel<T>
            {
                Data = default(T),
                ExceptionMessageList = new List<ResponseMessageModel>(),
                ResponseMessageList = new List<ResponseMessageModel>()
            };
        }

        public ResponseHelper<T> IncludingData(T data)
        {
            this.response.Data = data;

            return this;
        }

        public ResponseHelper<T> WithStatusCode(HttpStatusCode statusCode)
        {
            this.response.StatusCode = (int)statusCode;

            return this;
        }

        public ResponseHelper<T> IncludingMessage(string key)
        {
            var applicationResponse = ApplicationMessageHelper.GetApplicationResponseMessage(key);

            if (response != null)
            {
                this.response.ResponseMessageList = new List<ResponseMessageModel>() { applicationResponse };
            }

            return this;
        }

        public ResponseHelper<T> IncludingError(string key)
        {
            var applicationResponse = ApplicationMessageHelper.GetApplicationResponseMessage(key);

            if (response != null)
            {
                this.response.ExceptionMessageList = new List<ResponseMessageModel>() { applicationResponse };
            }

            return this;
        }

        public BaseServiceResponseModel<T> CreateResponse()
        {
            if (response.ExceptionMessageList.Any())
                response.StatusCode = (int)HttpStatusCode.BadRequest;

            return this.response;
        }
    }
}
