using Contact.Api.Common.Dto;
using Contact.Api.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Conracts
{
    public interface IContactService
    {
        BaseServiceResponseModel<ContactDetailDto> GetContact(string uuid);
        BaseServiceResponseModel<bool> AddContact(ContactModel contactModel);
        BaseServiceResponseModel<bool> UpdateContact(ContactDto contactDto);
        BaseServiceResponseModel<bool> DeleteContact(string uuid);
        BaseServiceResponseModel<List<ContactDto>> GetContacts();
        BaseServiceResponseModel<bool> AddContactInformation(ContactInformationModel contactInformationModel);
        BaseServiceResponseModel<bool> DeleteContactInformation(ContactInformationModel contactInformationModel);
    }
}
