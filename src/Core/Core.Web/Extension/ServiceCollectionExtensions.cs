using Core.Domain;
using Core.Service;
using Core.Utility;
using DotNetCore.CAP.Dashboard.GatewayProxy.Requester;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using System.Text.Json;
using Microsoft.OpenApi.Models;
using AutoMapper;

namespace Core.Web
{
    public static class ServiceCollectionExtensions
    {
        private static SiasunLogger logger = SiasunLogger.GetInstance(typeof(ServiceCollectionExtensions));

        private static bool IsDevelopment()
        {
            var env = Environment.GetEnvironmentVariable(RuningContextConstants.ASPNETCORE_ENVIRONMENT);
            return string.Equals(env, RuningContextConstants.DEV_ENVIRONMENT, StringComparison.OrdinalIgnoreCase);
        }

        #region Basic

        public static IServiceCollection AddRuntimeConfig<T>(this IServiceCollection services, IConfiguration config)
            where T : AllConfiguration
        {
            var obj = Activator.CreateInstance<T>();
            config.Bind(obj);
            RuntimeContext.Config = obj;
            RuntimeContext.Config.IsDevelopment = IsDevelopment();
            ConfigurationHelper.InitDBConnection(RuntimeContext.Config);
            //ConfigurationHelper.InitCache(RuntimeContext.Config);
            //if (RuntimeContext.Config.IsDevelopment)
            //{

            //    ConfigurationHelper.InitMQ(RuntimeContext.Config);
            //}
            return services;
        }

        public static IServiceCollection AddWebBasicConfig(this IServiceCollection services, Components component, IConfiguration config)
        {
            services.AddRuntimeConfig<AllConfiguration>(config);
            services.AddHttpContextAccessor()
                    .AddCustomHttpClient()
                    //.AddRequestLocalization(component)
                    //.AddAppMetrics(component)
                    .AddCorsPolicy();

            //services.AddDefaultCorrelationId(opts =>
            //{
            //    opts.RequestHeader = LoggerDefault.CorrelationIdRequestHeader;
            //    opts.AddToLoggingScope = true;
            //    opts.UpdateTraceIdentifier = true;
            //});

            //set  Language And Time
            //LanguageAndTimeSettingsUtil.GetLanguageAndTimeSettings();

            return services;
        }

        public static IServiceCollection AddAPIBasicConfig(this IServiceCollection services, Components component, IConfiguration config)
        {
            services.AddRuntimeConfig<AllConfiguration>(config);
            services.Configure<ApiBehaviorOptions>(option => option.SuppressModelStateInvalidFilter = true);
            services.AddControllers()
                    .AddNewtonsoftJson()
                    .AddJsonOptions(opts =>
                    {
                        opts.JsonSerializerOptions.DictionaryKeyPolicy = null;
                        opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                        //opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    });
                    //.AddMetrics();
            services.AddApiVersioning(opts =>
            {
                opts.ReportApiVersions = true;
                opts.AssumeDefaultVersionWhenUnspecified = true;
                opts.DefaultApiVersion = ApiVersion.Default;
            });
            services.AddCustomHttpClient();
            services.AddHttpContextAccessor();

            //services.AddDefaultCorrelationId(opts =>
            //{
            //    opts.RequestHeader = LoggerDefault.CorrelationIdRequestHeader;
            //    opts.AddToLoggingScope = true;
            //    opts.UpdateTraceIdentifier = true;
            //});
            //services.AddAppMetrics(component);
            //services.AddRequestLocalization(component);

            //set  Language And Time
            //LanguageAndTimeSettingsUtil.GetLanguageAndTimeSettings();

            return services;
        }

        public static IServiceCollection AddCustomHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpClient(HttpClientDefault.IgnoreDangerous, (c) => { })
                    .ConfigurePrimaryHttpMessageHandler(() =>
                    {
                        return new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        };
                    });
            services.AddHttpClient<IHttpClient, StandardHttpClient>();
            services.AddTransient<IHttpClient, StandardHttpClient>();
            return services;
        }

        //public static IServiceCollection AddAppMetrics(this IServiceCollection services, Components component)
        //{
        //    var metrics = AppMetrics.CreateDefaultBuilder()
        //                            .OutputMetrics.AsPrometheusPlainText()
        //                            .OutputMetrics.AsPrometheusProtobuf()
        //                            .Configuration.Configure(opts =>
        //                            {
        //                                opts.AddAppTag($"LEOGCC_{component}");
        //                            })
        //                            .Build();
        //    services.AddMetrics(metrics);
        //    services.AddMetricsTrackingMiddleware(opts =>
        //    {
        //        opts.ApdexTrackingEnabled = true;
        //        opts.ApdexTSeconds = 0.3;
        //        opts.IgnoredHttpStatusCodes = new List<int> { 404 };
        //        opts.IgnoredRoutesRegexPatterns = new List<string> { "(?i)^metrics-text$", "(?i)^metrics$", "(?i)^favicon.ico$" };
        //        opts.OAuth2TrackingEnabled = false;
        //    });
        //    services.AddMetricsEndpoints(opts =>
        //    {
        //        opts.EnvironmentInfoEndpointEnabled = false;
        //        opts.MetricsTextEndpointOutputFormatter = metrics.OutputMetricsFormatters.OfType<MetricsPrometheusTextOutputFormatter>().First();
        //        opts.MetricsEndpointOutputFormatter = metrics.OutputMetricsFormatters.OfType<MetricsPrometheusProtobufOutputFormatter>().First();
        //    });
        //    services.AddAppMetricsHealthPublishing();
        //    services.AddSingleton<IAppMetricsService, AppMetricsService>();
        //    return services;
        //}

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, string policyName = WebConstants.DefaultCorsPolicyName, CorsPolicy policy = null)
        {
            Action<CorsPolicyBuilder> defaultPolicy = null;
            if (IsDevelopment())
            {
                defaultPolicy =
                    builder => builder.SetIsOriginAllowed(a => true)
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .AllowCredentials();
            }
            else
            {
                var endPointInfo = RuntimeContext.Config.MicroServiceEndpoints;
                defaultPolicy =
                   builder => builder.WithOrigins(endPointInfo.System)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
            }

            services.AddCors(options =>
            {
                if (policy == null)
                {
                    options.AddPolicy(policyName, defaultPolicy);
                }
                else
                {
                    options.AddPolicy(policyName, policy);
                }
            });
            return services;
        }

        public static IHealthChecksBuilder AddServiceHealthCheck(this IServiceCollection services, Components component, List<string> checkServices = null)
        {
            if (checkServices == null)
            {
                checkServices = new List<string> { HealthCheckNodes.Endpoint, HealthCheckNodes.CoreSQL, HealthCheckNodes.SQL, HealthCheckNodes.Cache };
            }
            var checker = services.AddHealthChecks();

            if (checkServices.Contains(HealthCheckNodes.Endpoint))
            {
                checker.AddCheck("Endpoint", () => HealthCheckResult.Healthy());
            }

            var config = RuntimeContext.Config;
            var sqlProvider = SqlProviderFactory.Create();
            if (checkServices.Contains(HealthCheckNodes.SQL))
            {
                sqlProvider.AddHealthChecker(checker, config.DBConnections.AppDB, name: HealthCheckNodes.SQL, tags: new string[] { HealthCheckNodes.SQL });
            }

            if (checkServices.Contains(HealthCheckNodes.CoreSQL))
            {
                sqlProvider.AddHealthChecker(checker, config.DBConnections.CoreDB, name: HealthCheckNodes.CoreSQL, tags: new string[] { HealthCheckNodes.CoreSQL });
            }

            //if (checkServices.Contains(HealthCheckNodes.Cache))
            //{
            //    checker.Add(new HealthCheckRegistration(HealthCheckNodes.Cache, c => new CacheServerHealthCheck(), HealthStatus.Unhealthy, new string[] { HealthCheckNodes.Cache }));
            //}

            return checker;
        }

        //public static IServiceCollection AddRequestLocalization(this IServiceCollection services, Components component)
        //{
        //    services.Configure<RequestLocalizationOptions>(options =>
        //    {
        //        options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(Language.Default);
        //        options.SupportedCultures = Language.SupportedLanguageList;
        //        options.SupportedUICultures = Language.SupportedLanguageList;
        //        options.RequestCultureProviders.Clear();
        //        options.RequestCultureProviders.Add(new CustomizedRequestCultureProvider(options));
        //    });
        //    return services;
        //}
        #endregion

        #region Foundation
        //public static IServiceCollection AddCacheService(this IServiceCollection services)
        //{
        //    services.AddSingleton<ICacheService, CacheService>();
        //    CacheHelper.InitCacheHelper();
        //    return services;
        //}

        //public static IServiceCollection AddInitCommonUtil(this IServiceCollection services)
        //{
        //    services.AddSingleton<CommonUtil>();
        //    return services;
        //}

        //public static IServiceCollection AddAudit<T>(this IServiceCollection services, Components component)
        //    where T : ICapSubscribe
        //{
        //    //sqlprovider
        //    var sqlProvider = SqlProviderFactory.Create();
        //    var config = RuntimeContext.Config;
        //    services.AddEntityFrameworkSqlServer().AddDbContext<AuditDBContext>(options =>
        //    {
        //        sqlProvider.Use(options, config.DBConnections.AuditDB).UseLoggerFactory(AveLogger.AveLoggerFactory);
        //    }, ServiceLifetime.Scoped);

        //    //Config
        //    AuditConfig auditConfig = config.AuditServer;
        //    services.AddTransient<AuditDbOperator>();
        //    if (AuditHelp.IsDBTransmission(auditConfig))
        //    {
        //        //defualt db
        //        services.AddScoped<IAuditService, AuditDBService>();
        //    }
        //    else
        //    {
        //        services.AddScoped<IAuditService, AuditMQService>();
        //        //consumer
        //        if (auditConfig.UseModuleDB)
        //        {
        //            services.AddTransient(typeof(T));
        //        }
        //        else
        //        {
        //            //CommonService
        //            if (component.Equals(Components.CommonService))
        //            {
        //                services.AddTransient<AuditEventConsumer>();
        //            }
        //        }
        //    }

        //    return services;
        //}

        //public static IServiceCollection AddEventBus(this IServiceCollection services, string capschema = "cap")
        //{
        //    //sqlprovider
        //    var sqlProvider = SqlProviderFactory.Create();
        //    var config = RuntimeContext.Config;
        //    services.AddEntityFrameworkSqlServer().AddDbContext<CAPDBContext>(options =>
        //    {
        //        sqlProvider.Use(options, config.DBConnections.CAPDB).UseLoggerFactory(AveLogger.AveLoggerFactory);
        //    }, ServiceLifetime.Scoped);

        //    //Config
        //    var eventBusConfig = RuntimeContext.Config.MessageBusConfig;
        //    services.AddCap(x =>
        //    {
        //        x.DefaultGroupName = eventBusConfig.QueueName;
        //        sqlProvider.ConfigCAPSqlOptions(x, config.DBConnections.CAPDB, capschema);

        //        x.UseRabbitMQ(
        //            o =>
        //            {
        //                o.HostName = eventBusConfig.Host;
        //                o.Port = eventBusConfig.Port;
        //                o.UserName = eventBusConfig.Username;
        //                o.Password = eventBusConfig.Password;
        //                o.VirtualHost = eventBusConfig.VirtualHost;
        //                o.ExchangeName = eventBusConfig.VirtualHost;

        //            });

        //        x.UseDashboard(o =>
        //        {
        //            o.PathMatch = "/cap";
        //        });
        //        x.ConsumerThreadCount = eventBusConfig.ConsumerThreadCount;
        //        x.FailedRetryCount = eventBusConfig.RetryCount;
        //        x.FailedRetryInterval = eventBusConfig.RetryIntervalSeconds;
        //        x.FailedThresholdCallback = EventBusHelper.EventFailedThresholdCallback;
        //        //delete event message from database after 4 hours
        //        x.SucceedMessageExpiredAfter = 4 * 3600;
        //    });
        //    services.AddSingleton<IEventBus, EventBus>();
        //    return services;
        //}

        //public static IServiceCollection AddTimerJobRegister<T, TConsumer>(this IServiceCollection services)
        //    where T : BaseTimerRegisterService
        //    where TConsumer : class, ICapSubscribe
        //{
        //    services.AddTransient<TConsumer>();
        //    services.AddHostedService<T>();
        //    return services;
        //}

        public static IServiceCollection AddSwaggerDoc(this IServiceCollection services, Components component, string version, string description, string swaggerRelatedAssemblyName)
        {
            services.AddSwaggerGen(option =>
            {
                var docName = $"{component} {version}";
                option.SwaggerDoc(version, new OpenApiInfo
                {
                    Title = $"{component.ToString()} API",
                    Version = version,
                    Description = description,
                    Contact = new OpenApiContact
                    {
                        Name = "SiaSun",
                        Url = new Uri("https://www.siasun.com/")
                    }
                });
                option.DocumentFilter<HiddenAPIFilter>();
            });
            return services;
        }

        //public static IServiceCollection AddStorageService(this IServiceCollection services)
        //{
        //    services.AddTransient<IAzureBlobManager, AzureBlobManager>();
        //    services.AddTransient<ITempFileManager, TempFileManager>();
        //    services.AddGrpcClient<GStorage.GStorageClient>((sp, o) =>
        //                    GrpcClientFactoryOpts.Build(o, Components.StorageService, sp));
        //    return services;
        //}

        public static IServiceCollection AddGrpcOptions(this IServiceCollection services)
        {
            services.AddGrpc(s =>
            {
                s.EnableDetailedErrors = true;
                s.MaxReceiveMessageSize = int.MaxValue;
                s.MaxSendMessageSize = int.MaxValue;
            });

            return services;
        }
        #endregion

        #region EntityFramework Core
        public static IServiceCollection AddSqlServerContextPure<TContext, TRepository>(this IServiceCollection services, Components component)
            where TContext : DbContext where TRepository : class, IDBRepository
        {
            var sqlProvider = SqlProviderFactory.Create();
            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<TContext>(options =>
                    {
                        sqlProvider.Use(options, RuntimeContext.Config.DBConnections.AppDB).UseLoggerFactory(SiasunLogger.SiasunLoggerFactory);
                    }, ServiceLifetime.Scoped);
            services.AddSingleton<AutoMigration>();
            services.AddScoped<DbContext, TContext>();
            services.AddScoped<IDBRepository, TRepository>();
            return services;
        }

        public static IServiceCollection AddSqlServerContext<TContext, TRepository>(this IServiceCollection services, Components component)
            where TContext : DbContext where TRepository : class, IDBRepository
        {
            var sqlProvider = SqlProviderFactory.Create();
            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<TContext>(options =>
                    {
                        sqlProvider.Use(options, RuntimeContext.Config.DBConnections.AppDB).UseLoggerFactory(SiasunLogger.SiasunLoggerFactory);
                    }, ServiceLifetime.Scoped);
            services.AddSingleton<AutoMigration>();
            services.AddScoped<DbContext, TContext>();
            services.AddScoped<IDBRepository, TRepository>();
            //AddCoreDbContextInternal(services, component, sqlProvider);
            return services;
        }

        public static IServiceCollection AddAnotherDBContext<TContext>(this IServiceCollection services, Components component)
            where TContext : DbContext
        {
            var sqlProvider = SqlProviderFactory.Create();
            var dbConnectionConfig = RuntimeContext.Config.DBConnections;
            //var dbConnection = component == Components.AssessmentService ? dbConnectionConfig.FADB : dbConnectionConfig.AppDB;
            var dbConnection = string.Empty;
            switch (component)
            {
                //case Components.AssessmentService:
                //    dbConnection = dbConnectionConfig.FADB;
                //    break;
                //case Components.ProjectService:
                //    dbConnection = dbConnectionConfig.PADB;
                //    break;
                default:
                    dbConnection = dbConnectionConfig.AppDB;
                    break;
            }
            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<TContext>(options =>
                    {
                        sqlProvider.Use(options, dbConnection).UseLoggerFactory(SiasunLogger.SiasunLoggerFactory);
                    }, ServiceLifetime.Scoped);
            return services;
        }

        //public static IServiceCollection AddCoreDBContext(this IServiceCollection services, Components component)
        //{
        //    var sqlProvider = SqlProviderFactory.Create();
        //    AddCoreDbContextInternal(services, component, sqlProvider);
        //    return services;
        //}

        //private static void AddCoreDbContextInternal(IServiceCollection services, Components component, ISqlProvider sqlProvider)
        //{
        //    string coreDBConn = RuntimeContext.Config.DBConnections.CoreDB;
        //    if (!string.IsNullOrWhiteSpace(coreDBConn))
        //    {
        //        services.AddEntityFrameworkSqlServer()
        //                .AddDbContext<CoreDBContext>(options =>
        //                {
        //                    sqlProvider.Use(options, coreDBConn).UseLoggerFactory(AveLogger.AveLoggerFactory);
        //                }, ServiceLifetime.Scoped)
        //                .AddDbContextFactory<CoreDBThreadContext>(options =>
        //                {
        //                    sqlProvider.Use(options, coreDBConn).UseLoggerFactory(AveLogger.AveLoggerFactory);
        //                });

        //        services.AddSingleton<AutoMigration>();
        //        services.AddScoped<ICoreDBRepository, CoreDBRepository>();
        //        services.AddScoped<ICoreAsyncDBRepository, CoreAsyncDBRepository>();
        //        services.AddScoped<CoreDBContextFactory>();
        //        services.AddTransient<CacheCoreDBHelper>();
        //        services.AddTransient<CacheCoreAsyncDBHelper>();
        //    }
        //}

        public static IServiceCollection AddCoreServices(this IServiceCollection services, Components component)
        {
            //services.AddSingleton<IUrlService, UrlService>();
            //services.AddTransient<IPeoplePickerService, PeoplePickerService>();
            //services.AddSingleton<IExcelService, ExcelService>();

            //services.AddTransient<ISystemSettingService, SystemSettingService>();
            //services.AddTransient<ISettingService, SettingService>();

            //#region User
            //services.AddTransient<IStaffService, StaffService>();
            //services.AddTransient<IStaffIdMappingService, StaffIdMappingService>();
            //services.AddTransient<IStaffAccountMappingService, StaffAccountMappingService>();
            //services.AddTransient<IStudentService, StudentService>();
            //services.AddTransient<IUserService, UserService>();
            //services.AddTransient<IGroupService, GroupService>();
            //services.AddTransient<IStudentNoteService, StudentNoteService>();
            //#endregion

            //#region ACL
            //services.AddTransient<IUserRoleService, UserRoleService>();
            //services.AddTransient<IPermissionService, PermissionService>();
            //services.AddTransient<IUserRoleSimulationService, UserRoleSimulationService>();

            //services.AddTransient<IAclService, AclService>();
            //services.AddTransient<IResourceMapService, ResourceMapService>();

            //services.AddTransient<IGetAccountMappingService, GetAccountMappingService>();
            //#endregion

            //#region SQSP
            //services.AddTransient<ISchoolService, SchoolService>();
            //services.AddTransient<IQualificationTypeService, QualificationTypeService>();
            //services.AddTransient<ISemesterService, SemesterService>();
            //services.AddTransient<IProgrammeService, ProgrammeService>();
            //#endregion

            //#region Schedule
            //services.AddTransient<IScheduleService, ScheduleService>();
            //services.AddTransient<IPlanService, PlanService>();
            //services.AddTransient<IJobService, JobService>();
            //services.AddTransient<IJobDetailService, JobDetailService>();
            //#region Sync
            //services.AddTransient<ISyncReportService, SyncReportService>();
            //services.AddTransient<ISyncRecordService, SyncRecordService>();
            //#endregion
            //#endregion

            //#region Email
            //services.AddTransient<IEmailService, EmailService>();
            //services.AddTransient<ICommonEmailService, CommonEmailService>();
            //services.AddTransient<ILearningPathEmailRoleService, LearningPathEmailRoleService>();
            //#endregion

            //#region Common
            //services.AddTransient<IManagedLinkService, ManagedLinkService>();
            //services.AddTransient<ICommonTopicService, CommonTopicService>();
            //#endregion

            //#region ModuleMgt
            //services.AddTransient<IModuleService, ModuleService>();
            //services.AddTransient<IModuleCatalogService, ModuleCatalogService>();
            //services.AddTransient<IModuleStoreService, ModuleStoreService>();
            //services.AddTransient<IModuleScheduleService, ModuleScheduleService>();
            //services.AddTransient<IModuleGroupService, ModuleGroupService>();
            //services.AddTransient<ILessonService, LessonService>();
            //services.AddTransient<ILessonStrategyService, LessonStrategyService>();
            //services.AddTransient<ILearningPathService, LearningPathService>();
            //services.AddTransient<ILearningObjectService, LearningObjectService>();
            //services.AddTransient<ILearningObjectTypeService, LearningObjectTypeService>();
            //services.AddTransient<IClassLOService, ClassLOService>();
            //services.AddTransient<ILessonLearningObjectService, LessonLearningObjectService>();
            //services.AddTransient<IStudentLearningObjectService, StudentLearningObjectService>();
            //services.AddTransient<ILOSchedulerService, LOSchedulerService>();
            //#endregion

            //#region ClassMgt
            //services.AddTransient<IClassManagementService, ClassManagementService>();
            //services.AddTransient<IClassCommonService, ClassCommonService>();
            //services.AddTransient<IAttendanceService, AttendanceService>();
            //services.AddTransient<IClassService, ClassService>();
            //services.AddTransient<IClassRosterService, ClassRosterService>();
            //services.AddTransient<IClassLessonService, ClassLessonService>();
            //services.AddTransient<IClassLessonEventService, ClassLessonEventService>();
            //services.AddTransient<IClassManagementEmailRoleService, ClassManagementEmailRoleService>();
            //services.AddTransient<IVenueService, VenueService>();
            //services.AddTransient<ITeamAssignmentService, TeamAssignmentService>();
            //services.AddTransient<IClassMgtTimerService, ClassMgtTimerService>();
            //#endregion

            //#region Project
            //services.AddTransient<IPACoreService, PACoreService>();
            //#endregion

            //services.AddTransient<ISupplementaryListCommonService, SupplementaryCommonListService>();
            //services.AddTransient<IParticipantBridgeService, ParticipantBridgeService>();
            //services.AddTransient<IOutlookService, OutlookService>();
            //services.AddTransient<ICommonCalendarService, CommonCalendarService>();

            //services.AddTransient<IWorkflowService, WorkflowService>();
            //services.AddTransient<IWorkflowBaseService, WorkflowBaseService>();
            //services.AddTransient<IApprovalTaskRegister, ApprovalTaskRegister>();
            //services.AddTransient<ILessonTaskService, LessonTaskService>();

            //services.AddTransient<ILessonDurationService, LessonDurationService>();
            //services.AddTransient<IStudentLOAService, StudentLOAService>();
            //services.AddTransient<IAssessmentTeamService, AssessmentTeamService>();
            //services.AddTransient<IResourceSiteStatusService, ResourceSiteStatusService>();
            //services.AddTransient<IPolyAdminReportService, PolyAdminReportService>();
            //services.AddTransient<IExamService, ExamService>();
            //services.AddTransient<IPlagiarismCheckReportService, PlagiarismCheckReportService>();
            //services.AddTransient<IPlagiarismCheckVendor, TurnitinCheckVendor>();
            //services.AddTransient<IPlagiarismCheckService, PlagiarismCheckService>();
            //services.AddSingleton<ITimerJobService, TimerJobService>();
            //services.AddSingleton<ISendTimerMessageService, SendTimerMessageService>();
            //services.AddSingleton<IAuditDataQueryService, AuditDataQueryService>();

            //services.AddTransient<LearningObjectBridgeService>();
            //services.AddTransient<ChangeHandlerBridgeService>();
            //services.AddTransient<AssessmentEmailSetting>();
            //services.AddTransient<ILOGradeService, LOGradeService>();
            //services.AddTransient<IFATimerService, FATimerService>();
            //services.AddTransient<ILecturerAssignmentService, LecturerAssignmentService>();

            #region MyTasks
            //services.AddTransient<IMarkProgressService, MarkProgressService>();
            #endregion

            //services.AddTransient<IiCareSurveySessionService, iCareSurveySessionService>();

            #region for WorkFlow ServiceFactory               
            //services.AddTransient<WorkFlowServiceFactory>();
            #endregion

            //if (component == Components.WebService || component == Components.IdentityServer)
            //{
            //    services.AddGrpcClient<GStorage.GStorageClient>((sp, o) =>
            //        GrpcClientFactoryOpts.BuildForWeb(o, Components.StorageService, sp));
            //    services.AddGrpcClient<GTimer.GTimerClient>((sp, o) =>
            //        GrpcClientFactoryOpts.BuildForWeb(o, Components.TimerService, sp));
            //    services.AddGrpcClient<GAdmin.GAdminClient>((sp, o) =>
            //        GrpcClientFactoryOpts.BuildForWeb(o, Components.AdminService, sp));
            //    services.AddGrpcClient<GAssessmentRepository.GAssessmentRepositoryClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.AssessmentService, sp));
            //    services.AddGrpcClient<GAssessmentBaseService.GAssessmentBaseServiceClient>((sp, o) =>
            //        GrpcClientFactoryOpts.BuildForWeb(o, Components.AssessmentService, sp));
            //    services.AddGrpcClient<GPAParticipantService.GPAParticipantServiceClient>((sp, o) =>
            //        GrpcClientFactoryOpts.BuildForWeb(o, Components.ProjectService, sp));
            //    services.AddGrpcClient<GCalendar.GCalendarClient>((sp, o) =>
            //        GrpcClientFactoryOpts.BuildForWeb(o, Components.CollaborationService, sp));
            //    services.AddGrpcClient<GParticipant.GParticipantClient>((sp, o) =>
            //        GrpcClientFactoryOpts.BuildForWeb(o, Components.CollaborationService, sp));
            //    services.AddScoped<GrpcFactory>();
            //}
            //else
            //{
            //    services.AddGrpcClient<GStorage.GStorageClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.StorageService, sp));
            //    services.AddGrpcClient<GTimer.GTimerClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.TimerService, sp));
            //    services.AddGrpcClient<GAdmin.GAdminClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.AdminService, sp));
            //    services.AddGrpcClient<GAssessmentRepository.GAssessmentRepositoryClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.AssessmentService, sp));
            //    services.AddGrpcClient<GAssessmentBaseService.GAssessmentBaseServiceClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.AssessmentService, sp));
            //    services.AddGrpcClient<GFAWorkflowHandler.GFAWorkflowHandlerClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.AssessmentService, sp));
            //    services.AddGrpcClient<GPAParticipantService.GPAParticipantServiceClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.ProjectService, sp));
            //    services.AddGrpcClient<GFAWorkflowHandler.GFAWorkflowHandlerClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.AssessmentService, sp));
            //    services.AddGrpcClient<GPAParticipantService.GPAParticipantServiceClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.ProjectService, sp));
            //    services.AddGrpcClient<GCalendar.GCalendarClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.CollaborationService, sp));
            //    services.AddGrpcClient<GParticipant.GParticipantClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.CollaborationService, sp));
            //    services.AddScoped<GrpcFactory>();
            //}

            #region Async Service
            //services.AddSingleton<IUrlService, UrlService>();
            //services.AddTransient<IPeoplePickerService, PeoplePickerService>();
            //services.AddSingleton<IExcelService, ExcelService>();

            //services.AddTransient<ISystemSettingAsyncService, SystemSettingAsyncService>();
            //services.AddTransient<ISettingService, SettingService>();

            #region User
            //services.AddTransient<IStaffAsyncService, StaffAsyncService>();
            //services.AddTransient<IStaffIdMappingAsyncService, StaffIdMappingAsyncService>();
            //services.AddTransient<IStaffAccountMappingAsyncService, StaffAccountMappingAsyncService>();
            //services.AddTransient<IStudentAsyncService, StudentAsyncService>();
            //services.AddTransient<IUserAsyncService, UserAsyncService>();
            //services.AddTransient<IGroupAsyncService, GroupAsyncService>();
            //services.AddTransient<IStudentNoteService, StudentNoteService>();
            #endregion

            #region ACL
            //services.AddTransient<IUserRoleAsyncService, UserRoleAsyncService>();
            //services.AddTransient<IPermissionService, PermissionService>();
            //services.AddTransient<IUserRoleSimulationAsyncService, UserRoleSimulationAsyncService>();

            //services.AddTransient<IAclService, AclService>();
            //services.AddTransient<IResourceMapService, ResourceMapService>();

            //services.AddTransient<IGetAccountMappingService, GetAccountMappingService>();
            #endregion

            #region SQSP
            //services.AddTransient<ISchoolService, SchoolService>();
            //services.AddTransient<IQualificationTypeAsyncService, QualificationTypeAsyncService>();
            //services.AddTransient<ISemesterService, SemesterService>();
            //services.AddTransient<IProgrammeService, ProgrammeService>();
            #endregion

            //#region Schedule
            //services.AddTransient<IScheduleService, ScheduleService>();
            //services.AddTransient<IPlanService, PlanService>();
            //services.AddTransient<IJobService, JobService>();
            //services.AddTransient<IJobDetailService, JobDetailService>();
            //#region Sync
            //services.AddTransient<ISyncReportService, SyncReportService>();
            //services.AddTransient<ISyncRecordService, SyncRecordService>();
            //#endregion
            //#endregion

            //#region Email
            //services.AddTransient<IEmailService, EmailService>();
            //services.AddTransient<ICommonEmailService, CommonEmailService>();
            //services.AddTransient<ILearningPathEmailRoleService, LearningPathEmailRoleService>();
            //#endregion

            //#region Common
            //services.AddTransient<IManagedLinkService, ManagedLinkService>();
            //services.AddTransient<ICommonTopicService, CommonTopicService>();
            //#endregion

            //#region ModuleMgt
            //services.AddTransient<IModuleService, ModuleService>();
            //services.AddTransient<IModuleCatalogService, ModuleCatalogService>();
            //services.AddTransient<IModuleStoreService, ModuleStoreService>();
            //services.AddTransient<IModuleScheduleService, ModuleScheduleService>();
            //services.AddTransient<IModuleGroupService, ModuleGroupService>();
            //services.AddTransient<ILessonService, LessonService>();
            //services.AddTransient<ILessonStrategyService, LessonStrategyService>();
            //services.AddTransient<ILearningPathService, LearningPathService>();
            //services.AddTransient<ILearningObjectService, LearningObjectService>();
            //services.AddTransient<ILearningObjectTypeService, LearningObjectTypeService>();
            //services.AddTransient<IClassLOService, ClassLOService>();
            //services.AddTransient<ILessonLearningObjectService, LessonLearningObjectService>();
            //services.AddTransient<IStudentLearningObjectService, StudentLearningObjectService>();
            //services.AddTransient<ILOSchedulerService, LOSchedulerService>();
            //#endregion

            //#region ClassMgt
            //services.AddTransient<IClassManagementService, ClassManagementService>();
            //services.AddTransient<IClassCommonService, ClassCommonService>();
            //services.AddTransient<IAttendanceService, AttendanceService>();
            //services.AddTransient<IClassService, ClassService>();
            //services.AddTransient<IClassRosterService, ClassRosterService>();
            //services.AddTransient<IClassLessonService, ClassLessonService>();
            //services.AddTransient<IClassLessonEventService, ClassLessonEventService>();
            //services.AddTransient<IClassManagementEmailRoleService, ClassManagementEmailRoleService>();
            //services.AddTransient<IVenueService, VenueService>();
            //services.AddTransient<ITeamAssignmentService, TeamAssignmentService>();
            //services.AddTransient<IClassMgtTimerService, ClassMgtTimerService>();
            //#endregion

            //#region Project
            //services.AddTransient<IPACoreService, PACoreService>();
            //#endregion

            //services.AddTransient<ISupplementaryListCommonService, SupplementaryCommonListService>();
            //services.AddTransient<IParticipantBridgeService, ParticipantBridgeService>();
            //services.AddTransient<IOutlookService, OutlookService>();
            //services.AddTransient<ICommonCalendarService, CommonCalendarService>();

            //services.AddTransient<IWorkflowService, WorkflowService>();
            //services.AddTransient<IWorkflowBaseService, WorkflowBaseService>();
            //services.AddTransient<IApprovalTaskRegister, ApprovalTaskRegister>();
            //services.AddTransient<ILessonTaskService, LessonTaskService>();

            //services.AddTransient<ILessonDurationService, LessonDurationService>();
            //services.AddTransient<IStudentLOAService, StudentLOAService>();
            //services.AddTransient<IAssessmentTeamService, AssessmentTeamService>();
            //services.AddTransient<IResourceSiteStatusService, ResourceSiteStatusService>();
            //services.AddTransient<IPolyAdminReportService, PolyAdminReportService>();
            //services.AddTransient<IExamService, ExamService>();
            //services.AddTransient<IPlagiarismCheckReportService, PlagiarismCheckReportService>();
            //services.AddTransient<IPlagiarismCheckVendor, TurnitinCheckVendor>();
            //services.AddTransient<IPlagiarismCheckService, PlagiarismCheckService>();
            //services.AddSingleton<ITimerJobService, TimerJobService>();
            //services.AddSingleton<ISendTimerMessageService, SendTimerMessageService>();
            //services.AddSingleton<IAuditDataQueryService, AuditDataQueryService>();

            //services.AddTransient<LearningObjectBridgeService>();
            //services.AddTransient<ChangeHandlerBridgeService>();
            //services.AddTransient<AssessmentEmailSetting>();
            //services.AddTransient<ILOGradeService, LOGradeService>();
            //services.AddTransient<IFATimerService, FATimerService>();
            //services.AddTransient<ILecturerAssignmentService, LecturerAssignmentService>();

            //#region MyTasks
            //services.AddTransient<IMarkProgressService, MarkProgressService>();
            //#endregion

            //services.AddTransient<IiCareSurveySessionService, iCareSurveySessionService>();

            //#region for WorkFlow ServiceFactory               
            //services.AddTransient<WorkFlowServiceFactory>();
            //#endregion

            //if (component == Components.WebService || component == Components.IdentityServer)
            //{
            //    services.AddGrpcClient<GStorage.GStorageClient>((sp, o) =>
            //        GrpcClientFactoryOpts.BuildForWeb(o, Components.StorageService, sp));
            //    services.AddGrpcClient<GTimer.GTimerClient>((sp, o) =>
            //        GrpcClientFactoryOpts.BuildForWeb(o, Components.TimerService, sp));
            //    services.AddGrpcClient<GAdmin.GAdminClient>((sp, o) =>
            //        GrpcClientFactoryOpts.BuildForWeb(o, Components.AdminService, sp));
            //    services.AddGrpcClient<GAssessmentRepository.GAssessmentRepositoryClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.AssessmentService, sp));
            //    services.AddGrpcClient<GAssessmentBaseService.GAssessmentBaseServiceClient>((sp, o) =>
            //        GrpcClientFactoryOpts.BuildForWeb(o, Components.AssessmentService, sp));
            //    services.AddGrpcClient<GPAParticipantService.GPAParticipantServiceClient>((sp, o) =>
            //        GrpcClientFactoryOpts.BuildForWeb(o, Components.ProjectService, sp));
            //    services.AddGrpcClient<GCalendar.GCalendarClient>((sp, o) =>
            //        GrpcClientFactoryOpts.BuildForWeb(o, Components.CollaborationService, sp));
            //    services.AddGrpcClient<GParticipant.GParticipantClient>((sp, o) =>
            //        GrpcClientFactoryOpts.BuildForWeb(o, Components.CollaborationService, sp));
            //    services.AddScoped<GrpcFactory>();
            //}
            //else
            //{
            //    services.AddGrpcClient<GStorage.GStorageClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.StorageService, sp));
            //    services.AddGrpcClient<GTimer.GTimerClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.TimerService, sp));
            //    services.AddGrpcClient<GAdmin.GAdminClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.AdminService, sp));
            //    services.AddGrpcClient<GAssessmentRepository.GAssessmentRepositoryClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.AssessmentService, sp));
            //    services.AddGrpcClient<GAssessmentBaseService.GAssessmentBaseServiceClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.AssessmentService, sp));
            //    services.AddGrpcClient<GFAWorkflowHandler.GFAWorkflowHandlerClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.AssessmentService, sp));
            //    services.AddGrpcClient<GPAParticipantService.GPAParticipantServiceClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.ProjectService, sp));
            //    services.AddGrpcClient<GFAWorkflowHandler.GFAWorkflowHandlerClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.AssessmentService, sp));
            //    services.AddGrpcClient<GPAParticipantService.GPAParticipantServiceClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.ProjectService, sp));
            //    services.AddGrpcClient<GCalendar.GCalendarClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.CollaborationService, sp));
            //    services.AddGrpcClient<GParticipant.GParticipantClient>((sp, o) =>
            //        GrpcClientFactoryOpts.Build(o, Components.CollaborationService, sp));
            //    services.AddScoped<GrpcFactory>();
            //}
            #endregion

            return services;
        }
        #endregion

        #region Auto Mapper
        //public static IServiceCollection AddCoreAutoMapperConfig(this IServiceCollection services)
        //{
        //    try
        //    {
        //        services.AddAutoMapper(typeof(CoreMappingProfile));
        //        var mapperConfig = new MapperConfiguration(mc =>
        //        {
        //            mc.AddProfile(new CoreMappingProfile());
        //        });
        //        IMapper mapper = mapperConfig.CreateMapper();
        //        services.AddSingleton(mapper);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("An error occured while adding Core AutoMapper.", ex);
        //        throw;
        //    }
        //    return services;
        //}

        public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services, Type startupType, Profile profile)
        {
            try
            {
                services.AddAutoMapper(typeof(CoreMappingProfile), startupType);
                var mapperConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new CoreMappingProfile());
                    mc.AddProfile(profile);
                });
                IMapper mapper = mapperConfig.CreateMapper();
                services.AddSingleton(mapper);
            }
            catch (Exception ex)
            {
                logger.Error("An error occured while adding AutoMapper.", ex);
                throw;
            }
            return services;
        }

        //public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services, List<KeyValuePair<Type, Profile>> profiles)
        //{
        //    try
        //    {
        //        if (profiles.IsNullOrEmpty())
        //        {
        //            profiles = new List<KeyValuePair<Type, Profile>>();
        //        }
        //        profiles.Add(new KeyValuePair<Type, Profile>(typeof(CoreMappingProfile), new CoreMappingProfile()));

        //        services.AddAutoMapper(profiles.Select(a => a.Key).ToArray());
        //        var mapperConfig = new MapperConfiguration(mc =>
        //        {
        //            profiles.ForEach(a =>
        //            {
        //                mc.AddProfile(a.Value);
        //            });
        //        });
        //        IMapper mapper = mapperConfig.CreateMapper();
        //        services.AddSingleton(mapper);
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("An error occured while adding AutoMapper.", ex);
        //        throw;
        //    }
        //    return services;
        //}
        #endregion

        #region Authentication & Authorization
        //public static IServiceCollection AddAuthValidation(this IServiceCollection services, Components component)
        //{
        //    services.AddAuthForOAuth(component);
        //    return services;
        //}

        //private static IServiceCollection AddAuthForOAuth(this IServiceCollection services, Components component)
        //{
        //    var endPointInfo = RuntimeContext.Config.MicroServiceEndpoints;
        //    var cookieDomain = ConfigurationHelper.GetCookieDomain();
        //    string isUrl = endPointInfo.Identity?.Trim('/');
        //    string portalUrl = endPointInfo.Portal?.Trim('/');
        //    if (component == Components.IdentityServer)
        //    {
        //        services.AddAuthentication(options =>
        //        {
        //            options.DefaultAuthenticateScheme = LEOAuthenticationConstants.LoginCookieName;
        //            options.DefaultChallengeScheme = LEOAuthenticationConstants.LoginCookieName;
        //            options.DefaultSignInScheme = LEOAuthenticationConstants.LoginCookieName;
        //            options.DefaultSignOutScheme = LEOAuthenticationConstants.LoginCookieName;
        //        })
        //        .AddCookie(LEOAuthenticationConstants.LoginCookieName, options =>
        //        {
        //            options.Cookie.Domain = cookieDomain;
        //            options.Cookie.SameSite = SameSiteMode.None;
        //            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //            options.TicketDataFormat = new TicketDataFormat(new AesDataProtector());
        //            options.LoginPath = new PathString("/gotologin");
        //            options.Events.OnRedirectToLogin = (context) =>
        //            {
        //                string returnUrl = string.Empty;
        //                string redirectUrl = string.Empty;
        //                if (context.RedirectUri.EndsWith("/gotologin?ReturnUrl=%2F", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    returnUrl = endPointInfo.Portal.TrimEnd('/');
        //                    redirectUrl = endPointInfo.Portal.TrimEnd('/') + "/identity/login" + "?returnUrl=" + returnUrl;
        //                }
        //                else
        //                {
        //                    if (context.RedirectUri.Contains("/gotologin?ReturnUrl=%2F", StringComparison.OrdinalIgnoreCase))
        //                    {
        //                        returnUrl = $"{endPointInfo.Portal.TrimEnd('/')}/{context.RedirectUri.Substring(context.RedirectUri.IndexOf("/gotologin?ReturnUrl=%2F", StringComparison.OrdinalIgnoreCase) + 24)}";
        //                        redirectUrl = endPointInfo.Portal.TrimEnd('/') + "/identity/login" + "?returnUrl=" + returnUrl;
        //                    }
        //                    else
        //                    {
        //                        returnUrl = endPointInfo.Portal.TrimEnd('/');
        //                        redirectUrl = endPointInfo.Portal.TrimEnd('/') + "/login" + "?returnUrl=" + returnUrl;
        //                    }
        //                }

        //                context.Response.Redirect(redirectUrl.ToString());
        //                return Task.CompletedTask;
        //            };
        //            options.SlidingExpiration = false;
        //        })
        //        .AddCookie(LEOAuthenticationConstants.PermissionCookieName, options =>
        //        {
        //            options.Cookie.Domain = cookieDomain;
        //            options.Cookie.SameSite = SameSiteMode.None;
        //            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //            options.TicketDataFormat = new TicketDataFormat(new AesDataProtector());
        //            options.Events.OnRedirectToLogin = (context) =>
        //            {
        //                logger.Warn("**** No Permission Cookie, Unauthorized ****");
        //                context.Response.StatusCode = 401;
        //                return Task.CompletedTask;
        //            };
        //        })
        //        .AddCookie(LEOAuthenticationConstants.SingleSessionCookieName, options =>
        //        {
        //            options.Cookie.Domain = cookieDomain;
        //            options.Cookie.SameSite = SameSiteMode.None;
        //            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //            options.TicketDataFormat = new TicketDataFormat(new AesDataProtector());
        //            options.Events.OnRedirectToLogin = (context) =>
        //            {
        //                logger.Warn("**** No SingleSession Cookie, Unauthorized ****");
        //                context.Response.StatusCode = 401;
        //                return Task.CompletedTask;
        //            };
        //        })
        //        .AddOpenIdConnect(LEOOAuth2Constants.OAuth2AuthenticationScheme, LEOOAuth2Constants.OAuth2AuthenticationDisplayName, options =>
        //        {
        //            options.SignInScheme = LEOOAuth2Constants.OAuth2AuthenticationSignInScheme;
        //            options.SignOutScheme = LEOOAuth2Constants.OAuth2AuthenticationSignOutScheme;
        //            options.Authority = ConfigurationHelper.GetAuthority().TrimEnd('/') + '/' + ConfigurationHelper.GetTenantId().TrimStart('/');
        //            options.ClientId = ConfigurationHelper.GetClientId();
        //            options.ResponseType = "id_token";
        //            options.Scope.Add("openid");
        //            options.Scope.Add("profile");
        //            options.RequireHttpsMetadata = false;
        //            options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        //            {
        //                ValidateIssuer = false
        //            };
        //            options.GetClaimsFromUserInfoEndpoint = true;
        //            options.SaveTokens = true;

        //            AuthenticationEventsExtension ext = new AuthenticationEventsExtension();
        //            options.Events.OnTicketReceived = context => ext.TicketReceived(context);

        //            options.Events.OnRedirectToIdentityProvider = context =>
        //            {
        //                context.ProtocolMessage.RedirectUri = portalUrl + "/signin-oidc";
        //                return Task.CompletedTask;
        //            };
        //            options.Events.OnRedirectToIdentityProviderForSignOut = context =>
        //            {
        //                context.ProtocolMessage.PostLogoutRedirectUri = portalUrl + "/signout-callback-oidc";
        //                return Task.CompletedTask;
        //            };
        //            options.Events.OnSignedOutCallbackRedirect = context =>
        //            {
        //                logger.Info($"OnSignedOutCallbackRedirect, url is {context.Request.Scheme}://{context.Request.Host.Host}/{context.Request.Path}", context.ProtocolMessage);
        //                return Task.CompletedTask;
        //            };
        //            options.Events.OnRemoteSignOut = context =>
        //            {
        //                logger.Info($"OnRemoteSignOut, url is {context.Request.Scheme}://{context.Request.Host.Host}/{context.Request.Path}", context.ProtocolMessage);
        //                return Task.CompletedTask;
        //            };

        //            options.Events.OnAccessDenied = async context =>
        //            {
        //                logger.Error($"OnAccessDenied, url is {context.Request.Scheme}://{context.Request.Host.Host}/{context.Request.Path}", context.AccessDeniedPath);
        //                context.HandleResponse();
        //                await Task.CompletedTask;
        //            };
        //            options.Events.OnRemoteFailure = async context =>
        //            {
        //                logger.Error($"OnRemoteFailure, url is {context.Request.Scheme}://{context.Request.Host.Host}/{context.Request.Path}", context.Failure);
        //                context.Response.Redirect(portalUrl);
        //                context.HandleResponse();
        //                await Task.CompletedTask;
        //            };

        //            HttpClientHandler handler = new HttpClientHandler();
        //            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
        //            options.BackchannelHttpHandler = handler;

        //            ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //            {
        //                return true;
        //            };
        //        });

        //        services.AddTransient<LocalProvider>();
        //        services.AddTransient<AADProvider>();
        //        services.AddTransient<SimulateProvider>();
        //        services.AddTransient<SwitchProvider>();
        //        services.AddTransient<ILEOAuthorizationService, LEOAuthorizationService>();
        //        services.AddTransient<ILEOLoginURLService, LEOLoginURLService>();
        //        services.AddTransient<ILEOLoginService, LEOOAuth2LoginService>();

        //        services.AddPermissionControl();
        //    }
        //    else if (component == Components.WebService)
        //    {
        //        services.AddAuthentication(options =>
        //        {
        //            options.DefaultAuthenticateScheme = LEOAuthenticationConstants.LoginCookieName;
        //            options.DefaultChallengeScheme = LEOAuthenticationConstants.LoginCookieName;
        //            options.DefaultSignInScheme = LEOAuthenticationConstants.LoginCookieName;
        //            options.DefaultSignOutScheme = LEOAuthenticationConstants.LoginCookieName;
        //        })
        //        .AddCookie(LEOAuthenticationConstants.LoginCookieName, options =>
        //        {
        //            options.Cookie.Domain = cookieDomain;
        //            options.Cookie.SameSite = SameSiteMode.None;
        //            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //            options.TicketDataFormat = new TicketDataFormat(new AesDataProtector());
        //            options.LoginPath = new PathString("/gotologin");
        //            options.Events.OnRedirectToLogin = (context) =>
        //            {
        //                string returnUrl = string.Empty;
        //                string redirectUrl = string.Empty;
        //                if (context.RedirectUri.EndsWith("/gotologin?ReturnUrl=%2F", StringComparison.OrdinalIgnoreCase))
        //                {
        //                    returnUrl = endPointInfo.Portal.TrimEnd('/');
        //                    redirectUrl = endPointInfo.Portal.TrimEnd('/') + "/identity/login" + "?returnUrl=" + returnUrl;
        //                }
        //                else
        //                {
        //                    if (context.RedirectUri.Contains("/gotologin?ReturnUrl=%2F", StringComparison.OrdinalIgnoreCase))
        //                    {
        //                        returnUrl = $"{endPointInfo.Portal.TrimEnd('/')}/{context.RedirectUri.Substring(context.RedirectUri.IndexOf("/gotologin?ReturnUrl=%2F", StringComparison.OrdinalIgnoreCase) + 24)}";
        //                        redirectUrl = endPointInfo.Portal.TrimEnd('/') + "/identity/login" + "?returnUrl=" + returnUrl;
        //                    }
        //                    else
        //                    {
        //                        returnUrl = endPointInfo.Portal.TrimEnd('/');
        //                        redirectUrl = endPointInfo.Portal.TrimEnd('/') + "/login" + "?returnUrl=" + returnUrl;
        //                    }
        //                }

        //                context.Response.Redirect(redirectUrl.ToString());
        //                return Task.CompletedTask;
        //            };
        //            options.SlidingExpiration = false;
        //        })
        //        .AddCookie(LEOAuthenticationConstants.PermissionCookieName, options =>
        //        {
        //            options.Cookie.Domain = cookieDomain;
        //            options.Cookie.SameSite = SameSiteMode.None;
        //            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //            options.TicketDataFormat = new TicketDataFormat(new AesDataProtector());
        //            options.Events.OnRedirectToLogin = (context) =>
        //            {
        //                logger.Warn("**** No Permission Cookie, Unauthorized ****");
        //                context.Response.StatusCode = 401;
        //                return Task.CompletedTask;
        //            };
        //        })
        //        .AddCookie(LEOAuthenticationConstants.SingleSessionCookieName, options =>
        //        {
        //            options.Cookie.Domain = cookieDomain;
        //            options.Cookie.SameSite = SameSiteMode.None;
        //            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //            options.TicketDataFormat = new TicketDataFormat(new AesDataProtector());
        //            options.Events.OnRedirectToLogin = (context) =>
        //            {
        //                logger.Warn("**** No SingleSession Cookie, Unauthorized ****");
        //                context.Response.StatusCode = 401;
        //                return Task.CompletedTask;
        //            };
        //        });

        //        services.AddTransient<ILEOLoginURLService, LEOLoginURLService>();
        //    }
        //    else
        //    {
        //        services.AddAuthentication(options =>
        //        {
        //            options.DefaultAuthenticateScheme = LEOAuthenticationConstants.LoginCookieName;
        //            options.DefaultChallengeScheme = LEOAuthenticationConstants.LoginCookieName;
        //            options.DefaultSignInScheme = LEOAuthenticationConstants.LoginCookieName;
        //            options.DefaultSignOutScheme = LEOAuthenticationConstants.LoginCookieName;
        //        })
        //        .AddCookie(LEOAuthenticationConstants.LoginCookieName, options =>
        //        {
        //            options.Cookie.Domain = cookieDomain;
        //            options.Cookie.SameSite = SameSiteMode.None;
        //            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //            options.TicketDataFormat = new TicketDataFormat(new AesDataProtector());
        //            options.Events.OnRedirectToLogin = (context) =>
        //            {
        //                logger.Warn("**** No Login Cookie, Unauthorized ****");
        //                throw new LoginUnauthorizedException("Unauthorized");
        //            };
        //            options.SlidingExpiration = false;
        //        })
        //        .AddCookie(LEOAuthenticationConstants.PermissionCookieName, options =>
        //        {
        //            options.Cookie.Domain = cookieDomain;
        //            options.Cookie.SameSite = SameSiteMode.None;
        //            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //            options.TicketDataFormat = new TicketDataFormat(new AesDataProtector());
        //            options.Events.OnRedirectToLogin = (context) =>
        //            {
        //                logger.Warn("**** No Permission Cookie, Unauthorized ****");
        //                context.Response.StatusCode = 401;
        //                return Task.CompletedTask;
        //            };
        //        })
        //        .AddCookie(LEOAuthenticationConstants.SingleSessionCookieName, options =>
        //        {
        //            options.Cookie.Domain = cookieDomain;
        //            options.Cookie.SameSite = SameSiteMode.None;
        //            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        //            options.TicketDataFormat = new TicketDataFormat(new AesDataProtector());
        //            options.Events.OnRedirectToLogin = (context) =>
        //            {
        //                logger.Warn("**** No SingleSession Cookie, Unauthorized ****");
        //                context.Response.StatusCode = 401;
        //                return Task.CompletedTask;
        //            };
        //        });

        //        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        //                {
        //                    options.Authority = endPointInfo.Identity;
        //                    options.RequireHttpsMetadata = true;
        //                    options.Audience = "000000";
        //                    options.SaveToken = true;
        //                    options.Configuration = new OpenIdConnectConfiguration();
        //                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        //                    {
        //                        ValidIssuer = endPointInfo.Identity,
        //                        ValidateIssuer = false,
        //                        ValidAudience = endPointInfo.Identity,
        //                        ValidateAudience = false,
        //                        ValidateIssuerSigningKey = false,
        //                        ValidateLifetime = true,
        //                        ValidateActor = false,
        //                        ValidateTokenReplay = false,
        //                        ClockSkew = TimeSpan.Zero,
        //                        SignatureValidator = delegate (string token, Microsoft.IdentityModel.Tokens.TokenValidationParameters parameters)
        //                        {
        //                            var jwt = new JwtSecurityToken(token);
        //                            return jwt;
        //                        },
        //                    };
        //                    IdentityModelEventSource.ShowPII = true;
        //                    options.Events = new JwtBearerEvents
        //                    {
        //                        OnChallenge = op =>
        //                        {
        //                            op.HandleResponse();
        //                            op.Response.ContentType = "application/json";
        //                            op.Response.StatusCode = StatusCodes.Status401Unauthorized;
        //                            op.Response.WriteAsync(JsonConvert.SerializeObject(new
        //                            {
        //                                status = StatusCodes.Status401Unauthorized,
        //                                msg = "token invaild"
        //                            }));
        //                            return Task.CompletedTask;
        //                        }
        //                    };
        //                });

        //        services.AddTransient<LocalProvider>();
        //        services.AddTransient<AADProvider>();
        //        services.AddTransient<SimulateProvider>();
        //        services.AddTransient<SwitchProvider>();
        //        services.AddTransient<ILEOAuthorizationService, LEOAuthorizationService>();
        //        services.AddTransient<ILEOLoginURLService, LEOLoginURLService>();
        //        services.AddTransient<ILEOLoginService, LEOOAuth2LoginService>();

        //        services.AddPermissionControl();
        //    }
        //    services.AddSameSiteCookiePolicy();

        //    return services;
        //}
        #endregion
    }
}
