using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common
{
    public class ResponseMessageModel
    {
        [JsonProperty(PropertyName = "c")]
        public int Code { get; set; }
        [JsonProperty(PropertyName = "m")]
        public string Message { get; set; }
    }
}
