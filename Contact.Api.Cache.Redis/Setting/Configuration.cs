using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contact.Api.Cache.Redis
{
    public class Configuration
    {
        public List<RedisEndpoint> Sentinels { get; set; }
        public string Password { get; set; }
        public string ClusterName { get; set; }

        /// <summary>
        /// in milliseconds, default is 5000
        /// </summary>
        public int? ConnectionTimeout { get; set; }

        /// <summary>
        /// in milliseconds, default is 5000
        /// </summary>
        public int? SyncTimeout { get; set; }

        /// <summary>
        /// in seconds, default is 30
        /// </summary>
        public int? KeepAlive { get; set; }

        public int ConnectRetry { get; set; }
    }
}
