//using Microsoft.Extensions.Diagnostics.HealthChecks;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Core.Utility
//{
//    public class CacheServerHealthCheck : IHealthCheck
//    {
//        private static SiasunLogger logger = SiasunLogger.GetInstance(typeof(CacheServerHealthCheck));
//        private const string CacheHealthKey = "CacheHealthKey";

//        public CacheServerHealthCheck()
//        {
//        }

//        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            HealthCheckResult checkResult = HealthCheckResult.Unhealthy();
//            try
//            {
//                var cacheService = RuntimeContext.ServiceProvider.GetService(typeof(ICacheService)) as ICacheService;
//                cacheService.Get(CacheHealthKey);
//                checkResult = HealthCheckResult.Healthy();
//            }
//            catch (Exception ex)
//            {
//                checkResult = new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
//                logger.Error($"An error occured while checking cache server health status, cache server type.", ex);
//            }
//            return Task.FromResult(checkResult);
//        }
//    }
//}
