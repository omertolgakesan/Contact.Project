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
            CreateMap<InformationEntityModel, ContactInformationDto>();


            CreateMap<List<InformationEntityModel>, ContactInformationDto>()
            .ForMember(x => x.UUID, opt => opt.MapFrom(y => y.Select(z => z.UUID).FirstOrDefault()))
            .ForMember(x => x.Informations, opt => opt.MapFrom(y => y.Select(z =>
                  new InformationDto
                  {
                      InformationDescription = z.InformationDescription,
                      InformationType = z.ContactInformationType
                  })));


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
