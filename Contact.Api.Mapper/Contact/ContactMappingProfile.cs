using AutoMapper;
using Contact.Api.Common.Dto;
using Contact.Api.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Api.Mapper.Contact
{
    public class ContactMappingProfile : Profile
    {
        public ContactMappingProfile()
        {
            CreateMap<ContactModel, ContactEntityModel>();

            CreateMap<InformationModel, InformationEntityModel>();

            CreateMap<ContactEntityModel, ContactDto>();

            CreateMap<ContactEntityModel, ContactDetailDto>();

            CreateMap<InformationEntityModel, ContactInformationDto>();

        }
    }
}
