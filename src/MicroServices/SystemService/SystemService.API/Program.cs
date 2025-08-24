using Core.Domain;
using Core.Service;
using Core.Web;
using NLog;
using System.ComponentModel.Design;
using System.Reflection;
using static Azure.Core.HttpHeader;
using SystemService.Service;
using static Core.Domain.MicroServiceEndpointConstants;
using SystemService.Repository;
using Core.Utility;
using SystemService.API;
using Core.Grpc.Protos;

#region Predifine
string apiVersion = "v1";
Components systemServiceComonent = Components.SystemService;
Console.Title = ApplicationHostName.SystemService;
#endregion


#region Add basic services to the web application
WebApplicationBuilder builder = HostBuilderHelper.BuildAndRun(args, MicroServicePorts.System);

builder.Services
                .AddAPIBasicConfig(systemServiceComonent, builder.Configuration)
                //.AddCacheService()
                .AddGrpcOptions()
                .AddCorsPolicy()
                .AddSqlServerContext<SystemDBContext, DBRepository>(systemServiceComonent)
                .AddCoreServices(systemServiceComonent)
                //.AddAutoMapperConfig(typeof(MappingProfile), new MappingProfile())
                //.AddEventBus(Components.SystemService.ToDescription())
                //.AddAudit<AuditEventConsumer>(systemServiceComonent)
                //.AddAudit<AuditMapperService>(configureHelper)
                //.AddTimerJobRegister<TimerRegisterService, TimerEventConsumer>(configureHelper)
                //.AddAuthValidation(systemServiceComonent)
                .AddSwaggerDoc(systemServiceComonent, apiVersion, ComponentsDescription.SystemService, Assembly.GetExecutingAssembly().GetName().Name)
                .AddServiceHealthCheck(systemServiceComonent, new List<string> { HealthCheckNodes.Endpoint, HealthCheckNodes.CoreSQL, HealthCheckNodes.SQL, HealthCheckNodes.Cache });
#endregion


#region Add business services
builder.Services.AddTransient<ServiceFactory>();
builder.Services.AddTransient<IUserService, UserService>();



#endregion

#region Configuration the web application
WebApplication app = builder.Build();
IWebHostEnvironment env = builder.Environment;
IHostApplicationLifetime hostApplicationLifetime = app.Lifetime;


app.IsFoundationServiceStarted(MicroServicePorts.System, LogManager.GetCurrentClassLogger())
   .EnsureDBCreatedAndMigrated()
   .UseBasicConfig(env, systemServiceComonent)
   .UseSwaggerDoc(systemServiceComonent, apiVersion);

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    endpoints.MapGrpcService<GUserService>();
    //endpoints.MapGrpcService<GSLAAssessmentTaskService>();
    //endpoints.MapGrpcService<GSLACommonService>();
    //endpoints.MapGrpcService<GSLAJobDataService>();
    //endpoints.MapGrpcService<GSLAMarkFormService>();
    //endpoints.MapGrpcService<GSLAQuestionService>();
    //endpoints.MapGrpcService<GSLAStudentSubmissionService>();
    //endpoints.MapGrpcService<GSLASubmissionFormService>();
    //endpoints.MapGrpcService<GSLATaskService>();
    //endpoints.MapGrpcService<GCommonLOService>();
});

hostApplicationLifetime.ApplicationStarted.Register(() =>
{
    //ConfigurationHelper.OutputConfiguration();
    //LOTypesHelper.FeaturePath = Path.Combine(env.ContentRootPath, "App_Data");

    //using (var scope = app.Services.CreateScope())
    //{
    //    scope.ServiceProvider.GetRequiredService<ISubmissionCategoryService>().UpdateBuildinData();
    //    scope.ServiceProvider.GetRequiredService<ISubmissionFormService>().CreateDefaultForms(scope.ServiceProvider.GetRequiredService<DefaultFormsHelper>().Forms);
    //}
});

app.Run();
#endregion