using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Contact.Api.Common
{
    public class BaseServiceResponseModel<T> where T : new()
    {
        public BaseServiceResponseModel()
        {

        }

        [JsonProperty(PropertyName = "sc")]
        public int StatusCode { get; set; }
        [JsonProperty(PropertyName = "d")]
        public T Data { get; set; }
        [JsonProperty(PropertyName = "el")]
        public List<ResponseMessageModel> ExceptionMessageList { get; set; }
        [JsonProperty(PropertyName = "ml")]
        public List<ResponseMessageModel> ResponseMessageList { get; set; }
    }
}
