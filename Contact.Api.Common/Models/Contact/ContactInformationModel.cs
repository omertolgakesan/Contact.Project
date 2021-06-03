using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Models
{
    public class ContactInformationModel
    {
        public string UUID { get; set; }
        public List<InformationModel> InformationModels { get; set; }
    }
}
