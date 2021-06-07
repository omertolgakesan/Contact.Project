using System;
using System.Collections.Generic;
using System.Text;

namespace Contact.Api.Common.Conracts
{
    public interface IProducerManager
    {
        bool Enqueue<T>(IEnumerable<T> queueDataModels, string queueName) where T : class, new();
    }
}
