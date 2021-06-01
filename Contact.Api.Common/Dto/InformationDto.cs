using Contact.Api.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Dto
{
    public class InformationDto
    {
        public ContactInformationType InformationType { get; set; }
        public string InformationDescription { get; set; }
    }
}
