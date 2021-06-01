using Contact.Api.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Models
{
   public class InformationModel
    {
        public ContactInformationType ContactInformationType { get; set; }
        public string InformationDescription { get; set; }
    }
}
