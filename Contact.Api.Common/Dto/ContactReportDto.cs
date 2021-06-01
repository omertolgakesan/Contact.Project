using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Dto
{
    public class ContactReportDto
    {
        public string Location { get; set; }
        public List<ReportDto> Contacts { get; set; }

    }
}
