using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common
{
    public class AppSetting
    {
        public List<ConnectionString> ConnectionStrings { get; set; }
        public List<ApplicationResponse> ApplicationResponses { get; set; }
        public MongoSetting MongoSettings { get; set; }
        public string CurrentLanguage { get; set; }

    }
}
