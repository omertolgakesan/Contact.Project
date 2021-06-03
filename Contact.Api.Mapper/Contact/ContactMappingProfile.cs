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

            //CreateMap<List<InformationEntityModel>, ContactDetailDto>()
            //.ForMember(x => x.Informations, opt => opt.MapFrom(y => y.Select(z =>
            //      new ContactInformationDto
            //      {
            //          InformationDescription = z.InformationDescription,
            //          ContactInformationType = z.ContactInformationType
            //      })));


            CreateMap<List<ContactEntityModel>, ContactReportDto>()
            .ForMember(x => x.Contacts, opt => opt.MapFrom(x => x.Select(x =>
            new ReportDto
            {
                Firm = x.Firm,
                Lastname = x.Lastname,
                Name = x.Name,
                UUID = x.UUID
            }).ToList()));


        }
    }
}
