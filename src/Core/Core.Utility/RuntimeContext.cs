
using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public static class RuntimeContext
    {
        public static readonly SiasunLogger logger = SiasunLogger.GetInstance(MethodBase.GetCurrentMethod().DeclaringType);

        public static IServiceProvider ServiceProvider { get; set; }
        public static AllConfiguration Config { get; set; }
        
    }
    public class HealthCheckNodes
    {
        public const string SQL = "SQL Server";
        public const string CoreSQL = "Core SQL Server";
        public const string Endpoint = "Endpoint";
        public const string Grpc = "GrpcServer";
        public const string RabbitMQ = "RabbitMQServer";
        public const string Cache = "CacheServer";
    }
}
