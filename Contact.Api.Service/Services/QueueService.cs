using Contact.Api.Common.Conracts;
using Contact.Api.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Api.Service.Services
{

    public class QueueService : IQueueService
    {
        private readonly IProducerManager producerManager;
        public QueueService(IProducerManager iProducerManager)
        {
            producerManager = iProducerManager;
        }

        public bool LocationReport<T>(IEnumerable<T> queueDataModels) where T : class, new()
        {
           return producerManager.Enqueue<T>(queueDataModels, RabbitMQConsts.RabbitMqConstsList.ContactReport.ToString());
        }
    }
}
