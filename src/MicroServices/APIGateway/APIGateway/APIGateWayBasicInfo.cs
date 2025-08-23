using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace APIGateway
{
    public static class APIGateWayBasicInfo
    {
        public static WebApplicationBuilder CreateHostBuilder(string[] args, int port)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel((env, options) =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue;

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

        public class CertificateEncrypt
        {
            public static byte[] PemBytes(string fileName)
            {
                var str = File.ReadAllLines(fileName).Where(l => !l.Contains('-')).Where(l => !l.Contains(' '))
               .Aggregate("", (current, next) => current + next);
                return Convert.FromBase64String(str);
            }
        }
    }
}
