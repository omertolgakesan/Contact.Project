using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Conracts
{
   public interface IQueueService
    {
        bool LocationReport<T>(IEnumerable<T> queueDataModels) where T : class, new();
    }
}
