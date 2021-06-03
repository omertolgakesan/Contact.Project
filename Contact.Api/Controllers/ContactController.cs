using Contact.Api.Common;
using Contact.Api.Common.Conracts;
using Contact.Api.Common.Dto;
using Contact.Api.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService contactService;
        public ContactController(IContactService iContractService)
        {
            contactService = iContractService;
        }

        /// <summary>
        /// Get the contact information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public BaseServiceResponseModel<ContactDetailDto> GetContact(string uuid)
        {
            return contactService.GetContact(uuid);
        }

        /// <summary>
        /// ContactInformationType
        /// Phone:1
        /// Email:2
        /// Location:3
        /// </summary>
        /// <param name="contactModel"></param>
        /// <returns></returns>
        [HttpPost]
        public BaseServiceResponseModel<bool> AddContact(ContactModel contactModel)
        {
            return contactService.AddContact(contactModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactDto"></param>
        /// <returns></returns>
        [HttpPut]
        public BaseServiceResponseModel<bool> UpdateContact(ContactDto contactDto)
        {
            return contactService.UpdateContact(contactDto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UUID"></param>
        /// <returns></returns>
        [HttpDelete]
        public BaseServiceResponseModel<bool> DeleteContact(string uuid)
        {
            return contactService.DeleteContact(uuid);
        }

        /// <summary>
        /// Get the contact list
        /// </summary>
        /// <returns></returns>
        [HttpGet("ContactList")]
        public BaseServiceResponseModel<List<ContactDto>> GetContacts()
        {
            return contactService.GetContacts();
        }
       
        /// <summary>
        /// ContactInformationType
        /// Phone:1
        /// Email:2
        /// Location:3
        /// </summary>
        /// <param name="contactModel"></param>
        /// <returns></returns>
        [HttpPost("addcontactInformation")]
        public BaseServiceResponseModel<bool> AddContactInformation(ContactInformationModel contactInformationModel)
        {
            return contactService.AddContactInformation(contactInformationModel);
        }

        /// <summary>
        /// ContactInformationType
        /// Phone:1
        /// Email:2
        /// Location:3
        /// </summary>
        /// <param name="contactModel"></param>
        /// <returns></returns>
        [HttpDelete("deletecontactInformation")]
        public BaseServiceResponseModel<bool> DeleteContactInformation(ContactInformationModel contactInformationModel)
        {
            return contactService.DeleteContactInformation(contactInformationModel);
        }
    }
}
