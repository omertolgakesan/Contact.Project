using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Contact.Api.Common.Helper
{
    public static class ResponseMessageHelper<T>
        where T : new()
    {
        public static BaseServiceResponseModel<T> ResponseOk()
        {
            return new ResponseHelper<T>().CreateResponse();
        }

        public static BaseServiceResponseModel<T> ResponseError(string key, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ResponseHelper<T>().WithStatusCode(statusCode).IncludingError(key).CreateResponse();
        }

        public static BaseServiceResponseModel<T> ResponseOk(T data, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ResponseHelper<T>().WithStatusCode(statusCode).IncludingData(data).CreateResponse();
        }

        public static BaseServiceResponseModel<T> ResponseOk(T data, string key, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ResponseHelper<T>().WithStatusCode(statusCode).IncludingData(data).IncludingMessage(key).CreateResponse();
        }
    }
}
