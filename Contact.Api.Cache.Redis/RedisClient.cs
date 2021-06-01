using Contact.Api.Common.Conracts;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Contact.Api.Cache.Redis
{
    public class RedisClient : ICacheManager
    {
        public static ConnectionMultiplexer Client;
        private static string connectionString;
        private static Configuration configurationOptions;

        private const int defaultConnectionTimeout = 5000;
        private const int defaultSyncTimeout = 5000;
        private const int defaultKeepAlive = 30;
        private const string keyPrefix = "contact_master";


        public RedisClient(string connectionString)
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
                RedisClient.connectionString = connectionString;

            Connect();
        }

        public RedisClient(ConnectionMultiplexer connectionMultiplexer)
        {
            Client = connectionMultiplexer;
        }

        public RedisClient(Configuration configuration)
        {
            configurationOptions = configuration;
            Connect();
        }

        public static void Connect()
        {
            if (Client != null)
            {
                return;
            }

            if (configurationOptions != null)
            {
                var connectionMultiplexer = CreateConnectionAsSentinel(configurationOptions);
                var masterEndpoint = GetMasterEndpoint(configurationOptions, connectionMultiplexer);
                ConnectWithConfigurationOptions(configurationOptions, masterEndpoint);
            }
            else if (!string.IsNullOrWhiteSpace(connectionString))
            {
                Client = ConnectionMultiplexer.Connect(connectionString);
            }
            else
            {
                throw new RedisConnectionException(ConnectionFailureType.UnableToConnect, "A configuration or connection string couldn't be found to connect.");
            }
        }

        private static EndPoint GetMasterEndpoint(Configuration configuration, ConnectionMultiplexer connectionMultiplexer)
        {
            EndPoint masterEndpoint = null;

            if (!connectionMultiplexer.IsConnected)
            {
                Task.Delay(1000);
                if (!connectionMultiplexer.IsConnected)
                {
                    Task.Delay(1000);
                }
            }

            if (connectionMultiplexer.IsConnected)
            {
                var endpoints = connectionMultiplexer.GetEndPoints();
                foreach (var item in endpoints)
                {
                    try
                    {
                        var server = connectionMultiplexer.GetServer(item);
                        masterEndpoint = server.SentinelGetMasterAddressByName(configuration.ClusterName);

                        if (masterEndpoint != null)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO: logger
                    }
                }
            }
            else
            {
                //TODO: logger
            }

            return masterEndpoint;
        }

        private static ConnectionMultiplexer CreateConnectionAsSentinel(Configuration configuration)
        {
            var options = new ConfigurationOptions()
            {
                CommandMap = CommandMap.Sentinel,
                AllowAdmin = true,
                TieBreaker = "",
                ServiceName = "",
                SyncTimeout = configuration.SyncTimeout ?? defaultSyncTimeout,
                KeepAlive = configuration.KeepAlive ?? defaultKeepAlive,
                ConnectTimeout = configuration.ConnectionTimeout ?? defaultConnectionTimeout,
                ConnectRetry = configuration.ConnectRetry
            };

            configuration.Sentinels.ForEach((x) =>
            {
                options.EndPoints.Add(x.Host, x.Port);
            });

            ConnectionMultiplexer connectionMultiplexer = null;
            try
            {
                connectionMultiplexer = ConnectionMultiplexer.Connect(options);
            }
            catch (Exception ex)
            {
                //TODO: Logger eklenmeli.               
            }

            return connectionMultiplexer;
        }

        private static void ConnectWithConfigurationOptions(Configuration configuration, EndPoint masterEndpoint)
        {
            if (masterEndpoint != null)
            {
                Client = ConnectionMultiplexer.Connect(
                new ConfigurationOptions()
                {
                    CommandMap = CommandMap.Default,
                    EndPoints = { masterEndpoint },
                    TieBreaker = "",
                    ServiceName = "",
                    SyncTimeout = configuration.SyncTimeout ?? defaultSyncTimeout,
                    KeepAlive = configuration.KeepAlive ?? defaultKeepAlive,
                    ConnectTimeout = configuration.ConnectionTimeout ?? defaultConnectionTimeout,
                    ConnectRetry = configuration.ConnectRetry,
                    Password = configuration.Password
                });
            }
            else
            {
                //TODO: LOgger eklenmeli
            }
        }


        public T Get<T>(string key)
        {
            try
            {
                var db = Client.GetDatabase();
                var value = db.StringGet(keyPrefix + key).ToString();
                if (!string.IsNullOrWhiteSpace(value))
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            }
            catch (RedisTimeoutException ex)
            {
                //TODO Logger

                Client = null;
                Connect();
                if (Client.IsConnected)
                {
                    Get<T>(key);
                }
            }
            catch (RedisConnectionException ex)
            {
                //TODO Logger

                Client = null;
                Connect();
                if (Client.IsConnected)
                {
                    Get<T>(key);
                }
            }
            catch (Newtonsoft.Json.JsonReaderException ex)
            {
                //TODO Logger

                if (!Client.IsConnected)
                {
                    Client = null;
                    Connect();
                }
                else
                {
                    Remove(key);
                    return default(T);
                }
            }
            catch (RedisServerException ex)
            {
                //TODO Logger

                Client = null;
                Connect();
                if (Client.IsConnected)
                {
                    Get<T>(key);
                }
            }
            catch (SocketException ex)
            {
                //TODO Logger

                Client = null;
                Connect();
                if (Client.IsConnected)
                {
                    Get<T>(key);
                }
            }
            catch (Exception ex)
            {
                //TODO Logger

                Client = null;
                Connect();
                if (Client.IsConnected)
                {
                    Get<T>(key);
                }
            }

            return default(T);
        }

        public T Get<T>(string key, TimeSpan duration, Func<T> cacheFeed)
        {
            var cachedValue = Get<T>(key);

            if (cachedValue != null)
                return cachedValue;

            cachedValue = cacheFeed();
            Set<T>(key, cachedValue, DateTime.Now + duration);
            return cachedValue;
        }

        public void Remove(string key)
        {
            try
            {
                var db = Client.GetDatabase();
                db.KeyDelete(keyPrefix + key);
            }
            catch (RedisTimeoutException ex)
            {
                //TODO Logger

                Client = null;
                Connect();
                if (Client.IsConnected)
                {
                    Remove(key);
                }
            }
            catch (RedisConnectionException ex)
            {
                //TODO Logger

                Client = null;
                Connect();
                if (Client.IsConnected)
                {
                    Remove(key);
                }
            }
            catch (SocketException ex)
            {
                //TODO Logger

                Client = null;
                Connect();
                if (Client.IsConnected)
                {
                    Remove(key);
                }
            }
            catch (Exception ex)
            {
                //TODO Logger

                Client = null;
                Connect();
                if (Client.IsConnected)
                {
                    Remove(key);
                }
            }
        }

        public bool Set<T>(string key, T value, DateTime expiresAt)
        {
            var result = default(bool);
            try
            {
                var db = Client.GetDatabase();
                return db.StringSet(keyPrefix + key, JsonConvert.SerializeObject(value), expiresAt.Subtract(DateTime.Now));
            }
            catch (RedisTimeoutException ex)
            {
                //TODO: Logger

                Client = null;
                Connect();
                if (Client.IsConnected)
                {
                    result = Set<T>(key, value, expiresAt);
                }
                else
                {
                    result = false;
                }
            }
            catch (RedisConnectionException ex)
            {
                //TODO: Logger

                Client = null;
                Connect();
                if (Client.IsConnected)
                {
                    result = Set<T>(key, value, expiresAt);
                }
                else
                {
                    result = false;
                }
            }
            catch (SocketException ex)
            {
                //TODO: Logger

                Client = null;
                Connect();
                if (Client.IsConnected)
                {
                    result = Set<T>(key, value, expiresAt);
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                //TODO: Logger

                Client = null;
                Connect();
                if (Client.IsConnected)
                {
                    result = Set<T>(key, value, expiresAt);
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
