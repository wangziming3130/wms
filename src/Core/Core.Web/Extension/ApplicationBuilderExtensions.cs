using Core.Domain;
using Core.Service;
using Core.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using NLog;

namespace Core.Web
{
    public static class ApplicationBuilderExtensions
    {

        #region exception
        public static IApplicationBuilder UseHttpException(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HttpExceptionMiddleware>();
        }

        public static IApplicationBuilder UseApplicationException(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ApplicationExceptionMiddleware>();
        }

        #endregion

        public static IApplicationBuilder IsFoundationServiceStarted(this IApplicationBuilder app, int port, Logger logger)
        {
            if (!HostBuilderHelper.IsFoundationServiceStarted(app, port, logger))
            {
                throw new Exception($"The foundation service is not started. Current servic with port {port} could not be started.. Please check and run again.");
            }
            return app;
        }

        public static IApplicationBuilder UseBasicConfig(this IApplicationBuilder app, IWebHostEnvironment env,
            Components component, string policyName = WebConstants.DefaultCorsPolicyName)
        {
            RuntimeContext.ServiceProvider = app.ApplicationServices;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Map("/healthz", appconfig =>
            {
                appconfig.Run(async context =>
                {
                    await context.Response.WriteAsync("ok");
                });
            });

            app
               //.UseAppMetrics()
               .UseRouting()
               .UseStaticFiles()
               .UseCors(policyName);

            //filter get body
            app.Use(async (context, next) =>
            {
                context.Request.EnableBuffering();
                await next(context);
            });

            app.UseAuthentication()
               .UseApplicationException()
               .UseHttpException()
               .UseRequestLocalization();

            //app.UseCorrelationId();

            app.UseAuthorization();

            return app;
        }



        public static IApplicationBuilder UseSwaggerDoc(this IApplicationBuilder app, Components component, string version)
        {
            var routeUrl = MicroserviceEndpointInfo.GetRouteUrl(component);
            var endPointInfo = RuntimeContext.Config.MicroServiceEndpoints;
            var apigatewayUrl = endPointInfo.Portal;
            app.UseSwagger(
                c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = $"{apigatewayUrl}/{routeUrl}" },
                        new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" }
                    };
                });
            }
            );
            app.UseSwaggerUI(c =>
            {
                //user APIGateway
                c.SwaggerEndpoint($"/{routeUrl}/swagger/{version}/swagger.json", $"{component} API");
                //local test
                //c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{component} API");
            });
            return app;
        }

        //public static IApplicationBuilder UseServiceHealthCheck(this IApplicationBuilder app)
        //{
        //    app.UseHealthChecks("/api/health", new HealthCheckOptions()
        //    {
        //        Predicate = _ => true,
        //        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        //    });
        //    return app;
        //}

        public static IApplicationBuilder EnsureDBCreatedAndMigrated(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var autoMigration = serviceScope.ServiceProvider.GetRequiredService<AutoMigration>();
                autoMigration.EnsureDBCreatedAndMigrated();
            }
            return app;
        }

        //public static IApplicationBuilder EnsureCoreDBCreatedAndMigrated(this IApplicationBuilder app)
        //{
        //    using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        //    {
        //        var autoMigration = serviceScope.ServiceProvider.GetRequiredService<AutoMigration>();
        //        autoMigration.EnsureCoreDBCreatedAndMigrated();
        //    }
        //    return app;
        //}

        //public static IApplicationBuilder EnsureCAPDBCreatedAndMigrated(this IApplicationBuilder app)
        //{
        //    using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        //    {
        //        var autoMigration = serviceScope.ServiceProvider.GetRequiredService<AutoMigration>();
        //        autoMigration.EnsureCAPDBCreatedAndMigrated();
        //    }
        //    return app;
        //}

        //public static IApplicationBuilder EnsureAuditDBCreatedAndMigrated(this IApplicationBuilder app)
        //{
        //    using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        //    {
        //        var autoMigration = serviceScope.ServiceProvider.GetRequiredService<AutoMigration>();
        //        autoMigration.EnsureAuditDBCreatedAndMigrated();
        //    }
        //    return app;
        //}

        public static IEndpointRouteBuilder MapAuthenticationPages(this IEndpointRouteBuilder endpoint)
        {
            endpoint.Map("/signout", async context =>
            {
                var result = await context.AuthenticateAsync(AuthenticationConstants.JwtCookieScheme);
                if (result.Succeeded)
                {
                    await context.SignOutAsync();
                }
                context.Response.Redirect("/");
            });

            endpoint.Map("/signout-remote", async context =>
            {
                var result = await context.AuthenticateAsync(AuthenticationConstants.JwtCookieScheme);
                if (result.Succeeded)
                {
                    await context.SignOutAsync(AuthenticationConstants.OpenIdConnectScheme, new AuthenticationProperties()
                    {
                        RedirectUri = "/signout"
                    });
                }
                else
                {
                    context.Response.Redirect("/");
                }
            });

            endpoint.Map("/auth-transfer", async context =>
            {
                var result = await context.AuthenticateAsync(AuthenticationConstants.JwtCookieScheme);
                if (result.Succeeded)
                {
                    await context.Response.WriteAsync($"<script type=\"text/javascript\">var href = window.sessionStorage.getItem(\"{AuthenticationConstants.SessionStorageKey}\");" +
                        $"window.sessionStorage.removeItem(\"{AuthenticationConstants.SessionStorageKey}\");" +
                        "if(!href) {window.location.href = '/';}" +
                        "else if(window.location.href != href) {window.location.href = href;}</script>");
                }
                else
                {
                    await context.ChallengeAsync(AuthenticationConstants.OpenIdConnectScheme);
                }
            });
            return endpoint;
        }

        public static IApplicationBuilder UseRefererChecking(this IApplicationBuilder app)
        {
            var endpoints = RuntimeContext.Config.MicroServiceEndpoints;
            var tenantHost = string.IsNullOrWhiteSpace(endpoints.System) ? "" : endpoints.System.TrimEnd('/').ToLower();
            //var portalHost = string.IsNullOrWhiteSpace(endpoints.Portal) ? "" : endpoints.Portal.TrimEnd('/').ToLower();
            var checkMethodList = new List<string> { "post", "put", "delete" };
            app.Use(async (context, next) =>
            {
                var url = context.Request.Path.ToString().ToLower();
                var isNext = true;
                if (url.Contains("api"))
                {
                    var isValidReferer = false;
                    if (context.Request.Headers.TryGetValue("Referer", out var referer))
                    {
                        //isValidReferer = referer.Any(s => s.ToLower().StartsWith(tenantHost) || s.ToLower().StartsWith(portalHost));
                        isValidReferer = referer.Any(s => s.ToLower().StartsWith(tenantHost));
                    }
                    else
                    {
                        //if there is no referer in http request, the CUD option could not be completed.
                        var methodType = context.Request.Method.ToLower();
                        isValidReferer = !checkMethodList.Contains(methodType);
                    }
                    if (!isValidReferer)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync("Invalid referer information from the request!");
                        isNext = false;
                    }
                }
                if (isNext)
                {
                    await next();
                }
            });
            return app;
        }
    }
}
