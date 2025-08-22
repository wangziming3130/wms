using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace Core.Domain
{
    public class RuningContextConstants
    {
        public const string DEV_ENVIRONMENT = "Development";
        public const string ASPNETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";

        public static readonly TimeZoneInfo DefaultTimeZone = TZConvert.GetTimeZoneInfo("Singapore Standard Time");
    }

    public class SqlType
    {
        public const string SqlServer = "sqlserver";
        public const string PostgreSql = "postgresql";
    }
    public class LoggerDefault
    {
        public const string CorrelationIdRequestHeader = "X-Correlation-Id";
    }
}
