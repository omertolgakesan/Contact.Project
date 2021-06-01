using Contact.Api.Common.Dto;
using Contact.Api.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Conracts
{
    public interface IContactService
    {
        BaseServiceResponseModel<bool> AddContact(ContactModel contactModel);
        BaseServiceResponseModel<bool> DeleteContact(string UUID);
        BaseServiceResponseModel<List<ContactDto>> GetContacts();
        BaseServiceResponseModel<ContactInformationDto> GetContactInformations(string uUID);
        BaseServiceResponseModel<bool> UpdateContact(ContactDto contactDto);
    }
}
