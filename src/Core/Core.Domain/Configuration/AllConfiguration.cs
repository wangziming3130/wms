using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class AllConfiguration
    {
        public string ProductVersion { get; set; } = "1.0";
        public bool IsDevelopment { get; set; }
        public DBConnection DBConnections { get; set; }
        public string ELASTIC_POOL { get; set; }
        //public CacheConfigInfo CacheServer { get; set; }

        public MicroserviceEndpointInfo MicroServiceEndpoints { get; set; }
        public LoginConfig LoginConfiguration { get; set; }

        #region Database
        public string SqlType { get; set; }
        public string AuditConnectionString { get; set; }
        public string CAPDBConnectionString { get; set; }
        public string CoreConnectionString { get; set; }
        public string ConnectionString { get; set; }
        public string AssessmentConnectionString { get; set; }
        public string ProjectConnectionString { get; set; }
        #endregion
    }
    public class DBConnection
    {
        public string DBType { get; set; }

        public string CoreDB { get; set; }
        public string AppDB { get; set; }
        public string AuditDB { get; set; }
        public string CAPDB { get; set; }

        /// <summary>
        /// Only used in report service
        /// </summary>
        public string FADB { get; set; }
        public string PADB { get; set; }

        public string ELASTIC_POOL { get; set; }
    }
    public class LoginConfig
    {
        public string CookieDomain { get; set; } = ".rp.edu.sg";
        public string LoginCookieTimeOutInMinutes { get; set; } = "120";
        public string PermissionCookieTimeoutInMinutes { get; set; } = "120";
        public string EnableSingleSessionLogin { get; set; } = "false";
        public string SingleSessionCookieTimeoutInSeconds { get; set; } = "30";
        public string SingleSessionCacheTimeoutInMinutes { get; set; } = "30";
    }
    public class CacheConfigInfo
    {
        public string CacheName { get; set; }
        public string Password { get; set; }
        public int RetryCount { get; set; } = 5;
        public int ReconnectInterval { get; set; } = 60;
        public string ConnectionString { get; set; }
        public int DefaultExpirationTime { get; set; } = 120;
    }
}
