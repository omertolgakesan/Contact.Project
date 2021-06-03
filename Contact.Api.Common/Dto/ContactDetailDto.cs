using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Dto
{
    public class ContactDetailDto
    {
        public ContactDetailDto()
        {
            this.Informations = new List<ContactInformationDto>();
        }
        public string UUID { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Firm { get; set; }
        public List<ContactInformationDto> Informations { get; set; }
    }
}
