using Core.Domain;
using Core.Utility;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public static class GrpcClientFactoryOpts
    {
        private static readonly SiasunLogger logger = SiasunLogger.GetInstance(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        static GrpcClientFactoryOpts()
        {
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
        }

        public static void Build(GrpcClientFactoryOptions opts, Components targetComponent, IServiceProvider serviceProvider)
        {
            var microserviceInfo = RuntimeContext.Config.MicroServiceEndpoints;
            var targetServiceEndpoint = microserviceInfo.GetEndpoint(targetComponent);
            opts.Address = new Uri(targetServiceEndpoint);
            opts.ChannelOptionsActions.Add(a =>
            {
                a.MaxReceiveMessageSize = null;

                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                a.LoggerFactory = SiasunLogger.SiasunLoggerFactory;
                a.HttpClient = httpClientFactory.CreateClient(HttpClientDefault.IgnoreDangerous);

                a.HttpHandler = null;

                //a.Credentials = ChannelCredentials.Create(new SslCredentials(), CallCredentials.FromInterceptor((context, metadata) =>
                //{
                //    try
                //    {
                //        var _access = serviceProvider.GetRequiredService<ICorrelationContextAccessor>();
                //        var _correlationId = _access.CorrelationContext?.CorrelationId;
                //        if (!string.IsNullOrEmpty(_correlationId))
                //        {
                //            metadata.Add(LoggerDefault.CorrelationIdRequestHeader, _correlationId);
                //        }
                //    }
                //    catch
                //    {
                //        logger.Warn("An error occurred while Building in GrpcClientFactoryOpts");
                //    }

                //    return Task.CompletedTask;
                //}));
            });
        }

        //public static void BuildForWeb(GrpcClientFactoryOptions opts, Components targetComponent, IServiceProvider serviceProvider)
        //{
        //    var microserviceInfo = RuntimeContext.Config.MicroServiceEndpointsForGW;
        //    var targetServiceEndpoint = microserviceInfo.GetEndpoint(targetComponent);
        //    opts.Address = new Uri(targetServiceEndpoint);
        //    opts.ChannelOptionsActions.Add(a =>
        //    {
        //        a.MaxReceiveMessageSize = null;

        //        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        //        a.LoggerFactory = AveLogger.AveLoggerFactory;
        //        a.HttpClient = httpClientFactory.CreateClient(HttpClientDefault.IgnoreDangerous);

        //        a.HttpHandler = null;

        //        a.Credentials = ChannelCredentials.Create(new SslCredentials(), CallCredentials.FromInterceptor((context, metadata) =>
        //        {
        //            try
        //            {
        //                var _access = serviceProvider.GetRequiredService<ICorrelationContextAccessor>();
        //                var _correlationId = _access.CorrelationContext?.CorrelationId;
        //                if (!string.IsNullOrEmpty(_correlationId))
        //                {
        //                    metadata.Add(LoggerDefault.CorrelationIdRequestHeader, _correlationId);
        //                }
        //            }
        //            catch
        //            {
        //                logger.Warn("An error occurred while Building in GrpcClientFactoryOpts");
        //            }

        //            return Task.CompletedTask;
        //        }));
        //    });
        //}
    }
}
