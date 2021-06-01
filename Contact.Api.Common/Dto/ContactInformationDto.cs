using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Dto
{
    public class ContactInformationDto
    {
        public string UUID { get; set; }
        public List<InformationDto> Informations { get; set; }
    }
}
