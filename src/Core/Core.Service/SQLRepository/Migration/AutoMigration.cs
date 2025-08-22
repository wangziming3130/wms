
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.Loader;
using System.Text;
using Core.Utility;

namespace Core.Service
{
    public class AutoMigration
    {
        private static readonly SiasunLogger logger = SiasunLogger.GetInstance(typeof(AutoMigration));
        private const string _snapshotName = "ContextModelSnapshot";
        private IServiceProvider _serviceProvider;
        public AutoMigration(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void EnsureDBCreatedAndMigrated()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
                    EnsureDBCreatedAndMigrated(dbContext);
                    //var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();
                    //RunUpdateScript(cache, dbContext);
                    CreateELASTIC_POOL(dbContext);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"An error occured while trying to ensure DB exist and migrated.", ex);
                throw ex;
            }
        }

        //public void EnsureCoreDBCreatedAndMigrated()
        //{
        //    try
        //    {
        //        using (var scope = _serviceProvider.CreateScope())
        //        {
        //            var coreDBContext = scope.ServiceProvider.GetRequiredService<CoreDBContext>();
        //            EnsureDBCreatedAndMigrated(coreDBContext);
        //            var cache = scope.ServiceProvider.GetRequiredService<ICacheService>();
        //            RunUpdateScript(cache, coreDBContext, "Core");
        //            CreateAndRefreshFunctions(coreDBContext);
        //            CreateELASTIC_POOL(coreDBContext);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error($"An error occured while trying to ensure Core DB exist and migrated.", ex);
        //        throw ex;
        //    }
        //}

        //public void EnsureCAPDBCreatedAndMigrated()
        //{
        //    try
        //    {
        //        using (var scope = _serviceProvider.CreateScope())
        //        {
        //            if (!string.IsNullOrWhiteSpace(RuntimeContext.Config.DBConnections.CAPDB))
        //            {
        //                var capContext = scope.ServiceProvider.GetRequiredService<CAPDBContext>();
        //                EnsureDBCreatedAndMigrated(capContext);
        //                CreateELASTIC_POOL(capContext);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error($"An error occured while trying to ensure CAP DB exist and migrated.", ex);
        //        throw ex;
        //    }
        //}

        //public void EnsureAuditDBCreatedAndMigrated()
        //{
        //    try
        //    {
        //        using (var scope = _serviceProvider.CreateScope())
        //        {
        //            if (!string.IsNullOrWhiteSpace(RuntimeContext.Config.DBConnections.AuditDB))
        //            {
        //                var audtiContext = scope.ServiceProvider.GetRequiredService<AuditDBContext>();
        //                EnsureDBCreatedAndMigrated(audtiContext);
        //                CreateELASTIC_POOL(audtiContext);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error($"An error occured while trying to ensure Audit DB exist and migrated.", ex);
        //        throw ex;
        //    }
        //}

        public void EnsureDBCreatedAndMigrated(DbContext dbContext)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var migrationAssembly = dbContext.GetService<IMigrationsAssembly>();
                    var builder = new DesignTimeServicesBuilder(migrationAssembly.Assembly, Assembly.GetEntryAssembly(), new ModuleDbOperationReporter(), null);
                    var services = builder.Build(dbContext);
                    var dependencies = services.GetRequiredService<MigrationsScaffolderDependencies>();

                    var migratoinId = dependencies.MigrationsIdGenerator.GenerateId(dbContext.GetType().Name);
                    var isCreateNewDB = CheckOrCreateDB(dbContext, dependencies);
                    var historyRepository = dependencies.HistoryRepository as IMigrationHistoryRepository;

                    var migrationSql = GetMigrationSql(dbContext, historyRepository, migrationAssembly);
                    if (!string.IsNullOrWhiteSpace(migrationSql))
                    {
                        dbContext.Database.ExecuteSqlRaw(migrationSql);
                        AddMigrationRecord(dbContext, historyRepository, migratoinId);
                        dbContext.SaveChanges();
                    }
                    logger.Info($"Ensure DB exist and migrated operation has been completed. Context name:{dbContext.GetType().FullName}.");
                }
            }
            catch (Exception ex)
            {
                logger.Error($"An error occured while trying to ensure DB exist and migrated. Context name:{dbContext.GetType().FullName}", ex);
                throw ex;
            }
        }

        private bool CheckOrCreateDB(DbContext dbContext, MigrationsScaffolderDependencies dependencies)
        {
            var isCreateNew = false;
            var dbCreator = dbContext.Database.GetService<IRelationalDatabaseCreator>();
            if (!dbCreator.Exists())
            {
                dbCreator.Create();

                isCreateNew = true;
            }
            var migrationTableCommand = dependencies.HistoryRepository.GetCreateIfNotExistsScript();
            dbContext.Database.ExecuteSqlRaw(migrationTableCommand);
            dbContext.SaveChanges();
            return isCreateNew;
        }

        public string GetMigrationSql(DbContext dbContext, IMigrationHistoryRepository historyRepository, IMigrationsAssembly migrationAssembly)
        {
            var migrationSql = new StringBuilder();
            try
            {
                var lastMigrationCodes = historyRepository.GetLastMigrationCodes(dbContext);
                lastMigrationCodes = GZipCompress.GZipDecompressString(lastMigrationCodes);
                var addtionalAssemblies = new List<Assembly> { historyRepository.ValueGenerationStrategyAssembly(), migrationAssembly.Assembly };
                var lastModuleSnapShot = string.IsNullOrEmpty(lastMigrationCodes) ? null : CompileModelSnapshot(lastMigrationCodes, dbContext, addtionalAssemblies);

                //var dependencies = dbContext.GetService<ProviderConventionSetBuilderDependencies>();
                //var relationalDependencies = dbContext.GetService<RelationalConventionSetBuilderDependencies>();

                //var typeMappingConvention = new TypeMappingConvention(dependencies);
                //typeMappingConvention.ProcessModelFinalizing(((IConventionModel)lastModuleSnapShot.Model).Builder, null);

                //var relationalModelConvention = new RelationalModelConvention(dependencies, relationalDependencies);
                //var oldModel = relationalModelConvention.ProcessModelFinalized(lastModuleSnapShot.Model);

                var oldModel = lastModuleSnapShot?.Model;
                if (oldModel != null)
                {
                    oldModel = dbContext.GetService<IModelRuntimeInitializer>().Initialize(oldModel);
                }

                //var modelDiffer = dbContext.GetInfrastructure().GetService<IMigrationsModelDiffer>();
                //var hasDiffer = modelDiffer.HasDifferences(((IMutableModel)oldModel).FinalizeModel().GetRelationalModel(), 
                //                                           dbContext.GetService<IDesignTimeModel>().Model.GetRelationalModel());

                var modelDiffer = dbContext.GetService<IMigrationsModelDiffer>();
                var hasDiffer = modelDiffer.HasDifferences(oldModel?.GetRelationalModel(),
                                                           dbContext.GetService<IDesignTimeModel>().Model.GetRelationalModel());

                if (hasDiffer)
                {
                    //var upOperations = modelDiffer.GetDifferences(((IMutableModel)oldModel).FinalizeModel().GetRelationalModel(), 
                    //                                              dbContext.GetService<IDesignTimeModel>().Model.GetRelationalModel());
                    var upOperations = modelDiffer.GetDifferences(oldModel?.GetRelationalModel(),
                                                                  dbContext.GetService<IDesignTimeModel>().Model.GetRelationalModel());
                    var list = dbContext.GetInfrastructure()
                         .GetRequiredService<IMigrationsSqlGenerator>()
                         .Generate(upOperations, dbContext.GetService<IDesignTimeModel>().Model)
                         .Select(a => a.CommandText)
                         .ToList();
                    foreach (var sql in list)
                    {
                        migrationSql.AppendLine(sql);
                    }
                }
                logger.Info("Generate migration sql completed.");
                logger.Debug($"Generate migration sql completed. Sql:{migrationSql.ToString()}");
            }
            catch (Exception ex)
            {
                logger.Error("An error occured while trying to generate migration sql.", ex);
                throw ex;
            }
            return migrationSql.ToString();
        }

        private ModelSnapshot CompileModelSnapshot(string snapShotCodes, DbContext context, List<Assembly> addtionalAssemblies)
        {
            var currentDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assemblies = new HashSet<Assembly>()
                    {
                        currentDomainAssemblies.Where(a => a.GetName().Name == "netstandard").Single(),
                        currentDomainAssemblies.Where(a => a.GetName().Name == "System.Runtime").Single(),
                        typeof(object).Assembly,
                        typeof(DbContext).Assembly,
                        context.GetType().Assembly,
                        typeof(DbContextAttribute).Assembly,
                        typeof(ModelSnapshot).Assembly,
                        typeof(AssemblyTargetedPatchBandAttribute).Assembly
                    };
            if (addtionalAssemblies != null)
            {
                addtionalAssemblies.ForEach(a => assemblies.Add(a));
            }
            return CompileInstance<ModelSnapshot>(snapShotCodes, assemblies);
        }

        //private ModelSnapshot CreateModelSnapshot(string snapShotCodes, DbContext context, Assembly assembly)
        //{
        //    var references = GetType().Assembly
        //        .GetReferencedAssemblies()
        //        .Select(e => MetadataReference.CreateFromFile(Assembly.Load(e).Location))
        //        .Union(new MetadataReference[]
        //        {
        //            MetadataReference.CreateFromFile(Assembly.Load("netstandard").Location),
        //            MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
        //            MetadataReference.CreateFromFile(typeof(Object).Assembly.Location),
        //            MetadataReference.CreateFromFile(context.GetType().Assembly.Location),
        //            MetadataReference.CreateFromFile(assembly.Location)
        //        });
        //    var contextAssemblyName = context.GetType().Namespace + ".Migrations";
        //    var compilation = CSharpCompilation.Create(contextAssemblyName)
        //        .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
        //        .AddReferences(references)
        //        .AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(snapShotCodes));
        //    using (var stream = new MemoryStream())
        //    {
        //        var compileResult = compilation.Emit(stream);
        //        return compileResult.Success
        //            ? Assembly.Load(stream.GetBuffer()).CreateInstance(contextAssemblyName + "." + _snapshotName) as ModelSnapshot
        //            : null;
        //    }
        //}

        private T CompileInstance<T>(string source, IEnumerable<Assembly> references)
        {
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest);
            var compileOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                    .WithAssemblyIdentityComparer(DesktopAssemblyIdentityComparer.Default);
            var compilation = CSharpCompilation.Create("Dynamic", new[] { SyntaxFactory.ParseSyntaxTree(source, options) },
                references.Select(a => MetadataReference.CreateFromFile(a.Location)), compileOptions);
            using (var ms = new MemoryStream())
            {
                var e = compilation.Emit(ms);
                if (!e.Success)
                    throw new Exception("An error occued while trying to complile instance.");
                ms.Seek(0, SeekOrigin.Begin);
                var context = new AssemblyLoadContext(null, true);
                var assembly = context.LoadFromStream(ms);
                var modelType = assembly.DefinedTypes.Where(t => typeof(T).IsAssignableFrom(t)).Single();
                return (T)Activator.CreateInstance(modelType);
            }
        }

        private void AddMigrationRecord(DbContext dbContext, IMigrationHistoryRepository migrationHistory, string migrationId)
        {
            try
            {
                var contextType = dbContext.GetType();
                var contextFullname = contextType.FullName;
                logger.Info($"Begin to add migration records into database. Context name:{contextFullname}.");
                var contextAssemblyName = contextType.Namespace + ".Migrations";

                var snapshotCodes = new DesignTimeServicesBuilder(contextType.Assembly, contextType.Assembly,
                        new ModuleDbOperationReporter(), new string[0])
                            .Build(dbContext)
                            .GetService<IMigrationsCodeGenerator>()
                            .GenerateSnapshot(contextAssemblyName, contextType, _snapshotName, dbContext.GetService<IDesignTimeModel>().Model);

                snapshotCodes = GZipCompress.GZipCompressString(snapshotCodes);
                var productVersion = RuntimeContext.Config.ProductVersion;

                var info = new CDBMigrationInfo(migrationId, productVersion, contextFullname, snapshotCodes);
                migrationHistory.AddHistory(dbContext, info);
                logger.Info($"Add migration records into database completed. Context name:{contextFullname}.");
            }
            catch (Exception ex)
            {
                logger.Error($"An error occured while trying to add migration record to db.", ex);
                throw ex;
            }
        }

        private void CreateELASTIC_POOL(DbContext dbContext)
        {
            try
            {
                if (!string.IsNullOrEmpty(RuntimeContext.Config.ELASTIC_POOL))
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        dbContext.Database.ExecuteSqlRaw($@"BEGIN
                            DECLARE @db_name nvarchar(max) = DB_NAME();
                            IF SERVERPROPERTY('EngineEdition') = 5
                            EXEC(N'ALTER DATABASE [' + @db_name + '] MODIFY ( 
                            SERVICE_OBJECTIVE = ELASTIC_POOL ( name = [{RuntimeContext.Config.ELASTIC_POOL}] ) );')
                            END");
                        logger.Info($"Create ELASTIC_POOL: {RuntimeContext.Config.ELASTIC_POOL}. Context name:{dbContext.GetType().FullName}.");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"An error occured while trying to Create ELASTIC_POOL: {RuntimeContext.Config.ELASTIC_POOL}. Context name:{dbContext.GetType().FullName}", ex);
            }
        }


        //private void CreateAndRefreshFunctions(DbContext dbContext)
        //{
        //    try
        //    {
        //        using (var scope = _serviceProvider.CreateScope())
        //        {
        //            var database = dbContext.Database;
        //            database.ExecuteSqlRaw(@"if exists(select * from sys.objects where name = 'F_MyTaskFilterByAdmin')
        //                                         drop function F_MyTaskFilterByAdmin
        //                                         if exists(select * from sys.objects where name = 'F_MyTaskFilterByAcl')
        //                                         drop function F_MyTaskFilterByAcl
        //                                         if exists(select * from sys.objects where name = 'F_MyTaskDetailByAdmin')
        //                                         drop function F_MyTaskDetailByAdmin
        //                                         if exists(select * from sys.objects where name = 'F_MyTaskDetailByAcl')
        //                                         drop function F_MyTaskDetailByAcl
        //                                         if exists(select * from sys.objects where name = 'F_MyRecentTaskDetailByAcl')
        //                                         drop function F_MyRecentTaskDetailByAcl
        //                                         if exists(select * from sys.objects where name = 'F_LecturerTaskDetailByAdmin')
        //                                         drop function F_LecturerTaskDetailByAdmin
        //                                         if exists(select * from sys.objects where name = 'F_LecturerTaskDetailByAcl')
        //                                         drop function F_LecturerTaskDetailByAcl
        //                                         if exists(select * from sys.objects where name = 'F_MyTaskFilterByStudent')
        //                                         drop function F_MyTaskFilterByStudent");
        //            database.ExecuteSqlRaw(TaskFnSql.F_MyTaskFilterByAdmin);
        //            database.ExecuteSqlRaw(TaskFnSql.F_MyTaskFilterByAcl);
        //            database.ExecuteSqlRaw(TaskFnSql.F_MyTaskDetailByAdmin);
        //            database.ExecuteSqlRaw(TaskFnSql.F_MyTaskDetailByAcl);
        //            database.ExecuteSqlRaw(TaskFnSql.F_MyRecentTaskDetailByAcl);
        //            database.ExecuteSqlRaw(TaskFnSql.F_LecturerTaskDetailByAdmin);
        //            database.ExecuteSqlRaw(TaskFnSql.F_LecturerTaskDetailByAcl);
        //            database.ExecuteSqlRaw(TaskFnSql.F_MyTaskFilterByStudent);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error($"An error occured while trying to create functions to db.", ex);
        //        throw ex;
        //    }
        //}
        private void RunUpdateScript(/*ICacheService cache,*/ DbContext dbContext, string subFolder = "")
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Scripts", subFolder);
            if (!Directory.Exists(path))
            {
                logger.Info($"Skipping upgrading data, since no script folder was found.");
                return;
            }
            List<KeyValuePair<int, string>> scripts = new List<KeyValuePair<int, string>>();
            foreach (var filePath in Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly))
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var index = fileName.LastIndexOf('_');
                if (index > 0 && int.TryParse(fileName.Substring(index + 1), out int version))
                {
                    scripts.Add(new KeyValuePair<int, string>(version, filePath));
                }
            }
            if (scripts.Count > 0)
            {
                var scriptFiles = scripts.OrderBy(pair => pair.Key).Select(pair => pair.Value).ToList();
                var contextType = dbContext.GetType();
                //if (cache == null)
                //{
                    RunUpdateScriptInternal(dbContext, scriptFiles);
                //}
                //else
                //{
                //    var contextFullname = contextType.FullName;
                //    cache.LockKey(contextFullname, TimeSpan.FromSeconds(300), () =>
                //    {
                //        RunUpdateScriptInternal(dbContext, scriptFiles);
                //    });
                //}
            }
            else
            {
                logger.Info($"Skipping upgrading data, since no script was found.");
            }
        }

        private void RunUpdateScriptInternal(DbContext dbContext, List<string> scriptFiles)
        {
            foreach (var file in scriptFiles)
            {
                var script = File.ReadAllText(file, Encoding.UTF8);
                if (!string.IsNullOrWhiteSpace(script))
                {
                    dbContext.Database.ExecuteSqlRaw(script);
                }
            }
        }
    }

    public class ModuleDbOperationReporter : IOperationReporter
    {
        private static readonly SiasunLogger logger = SiasunLogger.GetInstance(MethodBase.GetCurrentMethod().DeclaringType);
        public void WriteError(string message)
        {
            logger.Error(message);
        }

        public void WriteInformation(string message)
        {
            logger.Info(message);

        }

        public void WriteVerbose(string message)
        {
            logger.Debug(message);
        }

        public void WriteWarning(string message)
        {
            logger.Warn(message);
        }
    }
}
