using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common
{
    public class ApplicationResponse
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
    }
}
