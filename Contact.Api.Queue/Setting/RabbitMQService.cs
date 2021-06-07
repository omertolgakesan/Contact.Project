using Contact.Api.Common;
using Contact.Api.Common.Conracts;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Contact.Api.Queue
{
    public class RabbitMQService : IRabbitMQService
    {

        private readonly IOptions<AppSetting> options;
        public RabbitMQService(IOptions<AppSetting> iOptions)
        {
            options = iOptions;
        }
        public IConnection GetConnection()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = options.Value?.QueueSettings.HostName,
                    UserName = options.Value?.QueueSettings.UserName,
                    Password = options.Value?.QueueSettings.Password,
                };

                factory.AutomaticRecoveryEnabled = true;
                factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);
                factory.TopologyRecoveryEnabled = false;

                return factory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                //TODO:Log
                Thread.Sleep(5000);
                return GetConnection();
            }
            catch (Exception ex)
            {
                //TODO:Log
                Thread.Sleep(5000);
                throw;
            }
        }

        public IModel GetModel(IConnection connection)
        {
            return connection.CreateModel();
        }
    }
}
