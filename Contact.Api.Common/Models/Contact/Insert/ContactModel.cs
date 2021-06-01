using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Models
{
    public class ContactModel
    {
        public ContactModel()
        {
            this.InformationModels = new List<InformationModel>();
        }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Firm { get; set; }
        public List<InformationModel> InformationModels { get; set; }
    }
}
