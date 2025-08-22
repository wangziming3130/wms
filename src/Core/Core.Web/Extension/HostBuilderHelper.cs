using Core.Utility;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.CodeAnalysis;
using NLog.LayoutRenderers;
using NLog.Targets.Wrappers;
using NLog;
using static Azure.Core.HttpHeader;
using static Core.Domain.MicroServiceEndpointConstants;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Security.Principal;
using Core.Domain;

namespace Core.Web
{
    public static class HostBuilderHelper
    {
        public static WebApplicationBuilder CreateHostBuilder(string[] args, int port)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel((env, options) =>
            {
                options.Limits.MaxRequestBodySize = WebConstants.DefaultMaxFileSize;

                //var filePath = env.Configuration["Certification:FilePath"];
                //var pemCertFileName = env.Configuration["Certification:LEOCrtFileName"];
                //var pemKeyFileName = env.Configuration["Certification:LEOKeyFileName"];
                //var pemCertFilePath = Path.Combine(filePath, pemCertFileName);
                //var pemKeyFilePath = Path.Combine(filePath, pemKeyFileName);
                //options.ConfigureHttpsDefaults(listenOptions =>
                //{
                //    using (var privateKey = RSA.Create())
                //    {
                //        privateKey.ImportRSAPrivateKey(CertificateEncrypt.PemBytes(pemKeyFilePath), out var bytesRead);
                //        X509Certificate2 certificate = new X509Certificate2(CertificateEncrypt.PemBytes(pemCertFilePath));
                //        listenOptions.ServerCertificate = new X509Certificate2(certificate.CopyWithPrivateKey(privateKey)
                //            .Export(X509ContentType.Pkcs12));
                //    }
                //});

                //if (env.HostingEnvironment.IsDevelopment() || port != 443)
                if (env.HostingEnvironment.IsDevelopment())
                {
                    options.Listen(IPAddress.Any, port, config =>
                    {
                        config.UseHttps();
                        config.Protocols = HttpProtocols.Http1AndHttp2;
                    });
                }
                else
                {
                    options.ConfigureEndpointDefaults(config =>
                    {
                        config.UseHttps();
                        config.Protocols = HttpProtocols.Http1AndHttp2;
                    });
                }

                options.AddServerHeader = false;
            });

            

            return builder;
        }

        public static WebApplicationBuilder BuildAndRun(string[] args, int port, bool checkServers = true)
        {
            string appName = AppDomain.CurrentDomain.FriendlyName;
            LayoutRenderer.Register<ELKLayoutRender>("elkmessage");
            var logger = LogManager.GetCurrentClassLogger();
            //todo
            //await EtcdHelper.OverrideEnvFromEtcd(logger);
            try
            {
                WebApplicationBuilder hostBuilder = CreateHostBuilder(args, port);
                if (checkServers)
                {
                    //var isServersReady = IsServersReady(hostBuilder);
                    //if (!isServersReady)
                    //{
                    //    throw new Exception("The cache server or event bus server could not be connected. Please check and run again.");
                    //}
                }
                return hostBuilder;
            }
            catch (Exception ex)
            {
                logger.Fatal($"Service {appName} exit with error. {ex}");
                throw;
            }
            finally
            {
                if (LogManager.Configuration != null && LogManager.Configuration.AllTargets.OfType<BufferingTargetWrapper>().Any())
                {
                    LogManager.Flush();
                }
            }
        }

        //private static bool IsServersReady(WebApplicationBuilder hostBuilder)
        //{
        //    var isReady = false;
        //    var isRedisReady = CacheService.IsRedisCacheReady(RuntimeContext.Config.CacheServer);
        //    if (isRedisReady)
        //    {
        //        //var rabbitMQConfig = RuntimeContext.Config.MessageBusConfig;
        //        //isReady = EventBusHelper.IsRabbitMQReady(rabbitMQConfig);
        //    }
        //    return isReady;
        //}

        public static bool IsFoundationServiceStarted(IApplicationBuilder app, int port, Logger logger)
        {
            var isStarted = false;

            switch (port)
            {
                case MicroServicePorts.APIGateway:
                //case MicroServicePorts.Timer:
                //case MicroServicePorts.Common:
                    {
                        isStarted = true;
                        break;
                    }
                case MicroServicePorts.Identity:
                case MicroServicePorts.System:
                //case MicroServicePorts.ModuleMgt:
                //case MicroServicePorts.ClassMgt:
                //case MicroServicePorts.LearningActivity:
                //case MicroServicePorts.Learning:
                //case MicroServicePorts.Assessment:
                //case MicroServicePorts.Collaboration:
                //case MicroServicePorts.Project:
                //case MicroServicePorts.Report:
                //case MicroServicePorts.Storage:
                    //{
                    //    using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                    //    {
                    //        var commonClient = serviceScope.ServiceProvider.GetRequiredService<GCommon.GCommonClient>();
                    //        isStarted = CheckClientStatus(port, commonClient.CheckStatus, logger);
                    //    }
                    //    break;
                    //}
                //case MicroServicePorts.Web:
                //    {
                //        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                //        {
                //            var identityClient = serviceScope.ServiceProvider.GetRequiredService<GIdentity.GIdentityClient>();
                //            isStarted = CheckClientStatus(port, identityClient.CheckStatus, logger);
                //        }
                //        break;
                //    }
                //case MicroServicePorts.IndustryStandard:
                //    {
                //        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                //        {
                //            var commonClient = serviceScope.ServiceProvider.GetRequiredService<GCommon.GCommonClient>();
                //            var storageClient = serviceScope.ServiceProvider.GetRequiredService<GStorage.GStorageClient>();
                //            isStarted = CheckClientStatus(port, commonClient.CheckStatus, logger) && CheckClientStatus(port, storageClient.CheckStatus, logger);
                //        }
                //        break;
                //    }
                default:
                    {
                        isStarted = true;
                        break;
                    }
            }
            return isStarted;
        }

        //private static bool CheckClientStatus(int port, Func<GBaseFilterMsg, Metadata, DateTime?, CancellationToken, GBaseFilterMsg> checkFunction, Logger logger)
        //{
        //    var isStarted = false;
        //    var intervalSecond = 3;
        //    var retryCount = 50;
        //    var para = new GBaseFilterMsg
        //    {
        //        Id = port.ToString()
        //    };
        //    while (retryCount != 0 && !isStarted)
        //    {
        //        try
        //        {
        //            var serviceStatus = checkFunction(para, null, null, default(System.Threading.CancellationToken));
        //            isStarted = serviceStatus.Status == 1;
        //        }
        //        catch
        //        {
        //            logger.Warn($"Check foundation service failed. Current port:{port}, function:{checkFunction.Method.DeclaringType.FullName} retry number:{retryCount}.");
        //        }
        //        finally
        //        {
        //            if (!isStarted)
        //            {
        //                Thread.Sleep(TimeSpan.FromSeconds(intervalSecond));
        //                retryCount--;
        //            }
        //        }
        //    }
        //    return isStarted;
        //}
    }
}
