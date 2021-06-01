using Contact.Api.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Conracts
{
    public interface IReportService
    {
        BaseServiceResponseModel<ContactReportDto> GetLocationReport(string location);
    }
}
