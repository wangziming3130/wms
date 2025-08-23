using APIGateway;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using System;
using System.IO;

#region Predifine
Console.Title = "API Gateway";
#endregion

#region Add services to the web application

WebApplicationBuilder builder = APIGateWayBasicInfo.CreateHostBuilder(args, 19100);

builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
{
    if (hostingContext.HostingEnvironment.EnvironmentName == "Development")
    {
        config.AddOcelot(Path.Combine(hostingContext.HostingEnvironment.ContentRootPath,
        "OcelotJson.Development"),
        hostingContext.HostingEnvironment);
    }
    else
    {
        config.AddOcelot(Path.Combine(hostingContext.HostingEnvironment.ContentRootPath,
        "OcelotJson"),
        hostingContext.HostingEnvironment);
    }
});

builder.Services.AddOcelot(builder.Configuration).AddPolly();
Action<CorsPolicyBuilder> defaultPolicy;

if (builder.Environment.EnvironmentName == "Development")
{
    defaultPolicy =
        builder => builder.SetIsOriginAllowed(a => true)
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowCredentials();
}
else
{
    var endPointInfo = builder.Configuration["MicroServiceEndpoints:Web"];
    defaultPolicy =
       builder => builder.WithOrigins(endPointInfo)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
}

builder.Logging.ClearProviders();
builder.Host.UseNLog();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CommonCorsPolicy", defaultPolicy);
});

#endregion

#region Configuration the web application
var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/healthz", async context =>
    {
        await context.Response.WriteAsync("ok");
    });
});

app.UseCors("CommonCorsPolicy");
app.UseOcelot().Wait();
app.Run();

#endregion
