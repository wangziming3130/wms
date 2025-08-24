using Core.Domain;
using Core.Service;
using Core.Web;
using NLog;
using System.ComponentModel.Design;
using System.Reflection;
using static Azure.Core.HttpHeader;
using WareHouseService.Service;
using static Core.Domain.MicroServiceEndpointConstants;
using WareHouseService.Repository;
using Core.Utility;
using Core.Grpc.Protos;

#region Predifine
string apiVersion = "v1";
Components wareHouseServiceComonent = Components.WareHouseService;
Console.Title = ApplicationHostName.WareHouseService;
#endregion


#region Add basic services to the web application
WebApplicationBuilder builder = HostBuilderHelper.BuildAndRun(args, MicroServicePorts.WareHouse);

builder.Services
                .AddAPIBasicConfig(wareHouseServiceComonent, builder.Configuration)
                //.AddCacheService()
                .AddGrpcOptions()
                .AddCorsPolicy()
                .AddSqlServerContext<WareHouseDBContext, DBRepository>(wareHouseServiceComonent)
                .AddCoreServices(wareHouseServiceComonent)
                //.AddAutoMapperConfig(typeof(MappingProfile), new MappingProfile())
                //.AddEventBus(Components.wareHouseService.ToDescription())
                //.AddAudit<AuditEventConsumer>(wareHouseService)
                //.AddAudit<AuditMapperService>(configureHelper)
                //.AddTimerJobRegister<TimerRegisterService, TimerEventConsumer>(configureHelper)
                //.AddAuthValidation(wareHouseService)
                .AddSwaggerDoc(wareHouseServiceComonent, apiVersion, ComponentsDescription.WareHouseService, Assembly.GetExecutingAssembly().GetName().Name)
                .AddServiceHealthCheck(wareHouseServiceComonent, new List<string> { HealthCheckNodes.Endpoint, HealthCheckNodes.CoreSQL, HealthCheckNodes.SQL, HealthCheckNodes.Cache });
#endregion


#region Add business services
builder.Services.AddTransient<ServiceFactory>();
builder.Services.AddTransient<ICellService, CellService>();
builder.Services.AddTransient<IWHService, WHService>();

builder.Services.AddGrpcClient<GUser.GUserClient>((sp, o) => GrpcClientFactoryOpts.Build(o, Components.SystemService, sp));
//ServiceLocator.Instance = builder.Services.BuildServiceProvider();
#endregion

#region Configuration the web application
WebApplication app = builder.Build();
IWebHostEnvironment env = builder.Environment;
IHostApplicationLifetime hostApplicationLifetime = app.Lifetime;


app.IsFoundationServiceStarted(MicroServicePorts.WareHouse, LogManager.GetCurrentClassLogger())
   .EnsureDBCreatedAndMigrated()
   .UseBasicConfig(env, wareHouseServiceComonent)
   .UseSwaggerDoc(wareHouseServiceComonent, apiVersion);

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
    //endpoints.MapGrpcService<GLearningObjectService>();
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