using Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    public class ConfigurationHelper
    {
        private static readonly SiasunLogger logger = SiasunLogger.GetInstance(typeof(ConfigurationHelper));

        public ConfigurationHelper()
        {
        }

        #region Login
        public static string GetCookieDomain()
        {
            try
            {
                var loginInfo = RuntimeContext.Config.LoginConfiguration;
                string returnvalue = loginInfo?.CookieDomain;
                if (string.IsNullOrEmpty(returnvalue))
                {
                    returnvalue = ".rp.edu.sg";
                }
                return returnvalue;
            }
            catch (Exception ex)
            {
                string defaultValue = ".rp.edu.sg";
                logger.Error($"ConfigurationHelper - CookieDomain Error: {ex}. Use Default:{defaultValue}");
                return defaultValue;
            }
        }
        public static int GetLoginCookieTimeOutInMinutes()
        {
            try
            {
                var loginInfo = RuntimeContext.Config.LoginConfiguration;
                int returnvalue = int.Parse(loginInfo?.LoginCookieTimeOutInMinutes);
                if (returnvalue < 1)
                {
                    returnvalue = 1;
                }
                return returnvalue;
            }
            catch (Exception ex)
            {
                int defaultValue = 120;
                //logger.Error($"ConfigurationHelper - LoginCookieTimeOutInMinutes Error: {ex}. Use Default:{defaultValue}");
                return defaultValue;
            }
        }
        public static int GetPermissionCookieTimeoutInMinutes()
        {
            try
            {
                var loginInfo = RuntimeContext.Config.LoginConfiguration;
                int returnvalue = int.Parse(loginInfo?.PermissionCookieTimeoutInMinutes);
                if (returnvalue < 1)
                {
                    returnvalue = 1;
                }
                return returnvalue;
            }
            catch (Exception ex)
            {
                int defaultValue = 120;
                //logger.Error($"ConfigurationHelper - PermissionCookieTimeoutInMinutes Error: {ex}. Use Default:{defaultValue}");
                return defaultValue;
            }
        }
        public static bool GetEnableSingleSessionLogin()
        {
            try
            {
                var loginInfo = RuntimeContext.Config.LoginConfiguration;
                bool returnvalue = bool.Parse(loginInfo?.EnableSingleSessionLogin);
                return returnvalue;
            }
            catch (Exception ex)
            {
                bool defaultValue = false;
                //logger.Error($"ConfigurationHelper - EnableSingleSessionLogin Error: {ex}. Use Default:{defaultValue}");
                return defaultValue;
            }
        }
        public static int GetSingleSessionCookieTimeoutInSeconds()
        {
            try
            {
                var loginInfo = RuntimeContext.Config.LoginConfiguration;
                int returnvalue = int.Parse(loginInfo?.SingleSessionCookieTimeoutInSeconds);
                if (returnvalue < 1)
                {
                    returnvalue = 1;
                }
                return returnvalue;
            }
            catch (Exception ex)
            {
                int defaultValue = 30;
                //logger.Error($"ConfigurationHelper - SingleSessionCookieTimeoutInSeconds Error: {ex}. Use Default:{defaultValue}");
                return defaultValue;
            }
        }
        public static int GetSingleSessionCacheTimeoutInMinutes()
        {
            try
            {
                var loginInfo = RuntimeContext.Config.LoginConfiguration;
                int returnvalue = int.Parse(loginInfo?.SingleSessionCacheTimeoutInMinutes);
                if (returnvalue < 1)
                {
                    returnvalue = 1;
                }
                return returnvalue;
            }
            catch (Exception ex)
            {
                int defaultValue = 30;
                //logger.Error($"ConfigurationHelper - SingleSessionCacheTimeoutInMinutes Error: {ex}. Use Default:{defaultValue}");
                return defaultValue;
            }
        }
        #endregion

        #region OAuth2
        //public static string GetAuthority()
        //{
        //    try
        //    {
        //        var oAuth2Info = RuntimeContext.Config.OAuth2Configuration;
        //        string returnvalue = oAuth2Info?.Authority;
        //        if (string.IsNullOrEmpty(returnvalue))
        //        {
        //            returnvalue = "https://login.microsoftonline.com/";
        //        }
        //        return returnvalue;
        //    }
        //    catch (Exception ex)
        //    {
        //        string defaultValue = "https://login.microsoftonline.com/";
        //        logger.Error($"ConfigurationHelper - GetAuthority Error: {ex}. Use Default:{defaultValue}");
        //        return defaultValue;
        //    }
        //}
        //public static string GetTenantId()
        //{
        //    try
        //    {
        //        var oAuth2Info = RuntimeContext.Config.OAuth2Configuration;
        //        return oAuth2Info?.TenantId;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error($"ConfigurationHelper - GetTenantId Error: {ex}");
        //        return string.Empty;
        //    }
        //}
        //public static string GetClientId()
        //{
        //    try
        //    {
        //        var oAuth2Info = RuntimeContext.Config.OAuth2Configuration;
        //        return oAuth2Info?.ClientId;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error($"ConfigurationHelper - GetClientId Error: {ex}");
        //        return string.Empty;
        //    }
        //}
        #endregion

        #region Survey
        //public static string SSInternalStaffEmail()
        //{
        //    var surveyConfig = RuntimeContext.Config.SurveyConfig;
        //    string SSInternalStaffEmailString = surveyConfig.SSInternalStaffEmail;
        //    logger.Info($"ConfigurationHelper - SSInternalStaffEmail : {SSInternalStaffEmailString}");
        //    return SSInternalStaffEmailString;
        //}

        //public static string SSInternalStaffEmail2()
        //{
        //    var surveyConfig = RuntimeContext.Config.SurveyConfig;
        //    string SSInternalStaffEmailString = surveyConfig.SSInternalStaffEmail2;
        //    logger.Info($"ConfigurationHelper - SSInternalStaffEmail2 : {SSInternalStaffEmailString}");
        //    return SSInternalStaffEmailString;
        //}
        #endregion

        #region Icare
        //public static string GetIcareHost()
        //{
        //    var iCareConfig = RuntimeContext.Config.IcareConfig;
        //    string host = iCareConfig.Host;
        //    logger.Info($"ConfigurationHelper - IcareConfig Host : {host}");
        //    return host;
        //}
        #endregion

        #region Database Connection String
        public static void InitDBConnection(AllConfiguration config)
        {
            if (config.DBConnections == null)
            {
                config.DBConnections = new DBConnection();
            }
            config.DBConnections.DBType = config.SqlType;
            if (string.IsNullOrWhiteSpace(config.DBConnections.DBType))
            {
                config.DBConnections.DBType = "sqlserver";
            }
            //config.DBConnections.CoreDB = DataProtection.ResolveConnectionString(config.CoreConnectionString);
            config.DBConnections.AppDB = DataProtection.ResolveConnectionString(config.ConnectionString);
            //if (config.AuditServer != null && config.AuditServer.UseModuleDB)
            //{
            //    config.DBConnections.AuditDB = config.DBConnections.AppDB;
            //}
            //else
            //{
            //    config.DBConnections.AuditDB = DataProtection.ResolveConnectionString(config.AuditConnectionString);
            //}
            //config.DBConnections.CAPDB = DataProtection.ResolveConnectionString(config.CAPDBConnectionString);
            //if (string.IsNullOrWhiteSpace(config.DBConnections.CAPDB))
            //{
            //    config.DBConnections.CAPDB = config.DBConnections.CoreDB;
            //}
            //config.DBConnections.FADB = DataProtection.ResolveConnectionString(config.AssessmentConnectionString);
            //config.DBConnections.PADB = DataProtection.ResolveConnectionString(config.ProjectConnectionString);
            config.DBConnections.ELASTIC_POOL = config.ELASTIC_POOL;
        }

        public static string DBConnection(Nullable<Components> component = null)
        {
            string connectionValue = string.Empty;
            if (component == null)
            {
                connectionValue = RuntimeContext.Config.DBConnections.AppDB;
            }
            else
            {
                switch (component)
                {
                    //case Components.AssessmentService:
                    //    {
                    //        connectionValue = RuntimeContext.Config.DBConnections.FADB;
                    //    }
                    //    break;
                    //case Components.ProjectService:
                    //    {
                    //        connectionValue = RuntimeContext.Config.DBConnections.PADB;
                    //    }
                    //    break;
                    default:
                        break;
                }
            }
            return connectionValue;
        }
        #endregion

        //public static void InitCache(AllConfiguration config)
        //{
        //    if (config.CacheServer != null)
        //    {
        //        if (!string.IsNullOrEmpty(config.CacheServer?.Password))
        //        {
        //            config.CacheServer.ConnectionString = config.CacheServer.ConnectionString.TrimEnd(new char[] { ' ', ',' }) + ",password=" + config.CacheServer.Password;
        //        }
        //    }
        //}

        #region MQ
        //public static void InitMQ(AllConfiguration config)
        //{
        //    if (config.MessageBusConfig != null)
        //    {
        //        config.MessageBusConfig.QueueName = "cap." + config.MessageBusConfig.QueueName;
        //        if ((string.IsNullOrWhiteSpace(config.MessageBusConfig.VirtualHost) || config.MessageBusConfig.VirtualHost.ToLower().Equals("localhost")))
        //        {
        //            logger.Info($"The virtual host is not valid and use local IP address instead.");
        //            config.MessageBusConfig.VirtualHost = "cap." + NetWork.LocalIPAddress;
        //            EnsureRabbitMQVirtualHost(config.MessageBusConfig);
        //        }
        //    }
        //}

        //private static bool EnsureRabbitMQVirtualHost(EventBusConfig config)
        //{
        //    try
        //    {
        //        var credentials = new NetworkCredential() { UserName = config.Username, Password = config.Password };
        //        using (var handler = new HttpClientHandler { Credentials = credentials })
        //        using (var client = new System.Net.Http.HttpClient(handler))
        //        {
        //            #region Create new virtual host by IP address for each environment
        //            var url = $"http://{config.Host}:{config.ManagePort}/api/vhosts/{config.VirtualHost}";

        //            var content = new StringContent("", Encoding.UTF8, "application/json");
        //            var result = client.PutAsync(url, content).Result;

        //            if ((int)result.StatusCode >= 300)
        //                throw new Exception(result.ToString());

        //            logger.Info($"The virtual host({config.VirtualHost}) does exist.");
        //            #endregion

        //            #region Grant admin permission for the RabbitMq connection account.
        //            url = $"http://{config.Host}:{config.ManagePort}/api/permissions/{config.VirtualHost}/{config.Username}";
        //            string perStr = $"{{\"vhost\":\"{config.VirtualHost}\",\"username\":\"{config.Username}\",\"configure\":\".*\",\"write\":\".*\",\"read\":\".*\"}}";
        //            content = new StringContent(perStr, Encoding.UTF8, "application/json");
        //            result = client.PutAsync(url, content).Result;

        //            if ((int)result.StatusCode >= 300)
        //                throw new Exception(result.ToString());

        //            logger.Info($"The user permission({config.Username}) of virtual host({config.VirtualHost}) is granted.");
        //            #endregion

        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error($"An error occured while trying to ensure the virtual host", ex);
        //        return false;
        //    }
        //}

        #endregion

        #region Storage
        //public static string GetFileSharePath(string component)
        //{
        //    logger.Info($"The system starts to obtain the FileShare base path. component : {component}");
        //    var path = RuntimeContext.Config.FileShareConfig.TempFilePath;
        //    var result = Path.Combine(path, component.TrimStart('\\'));
        //    if (!Directory.Exists(result))
        //    {
        //        Directory.CreateDirectory(result);
        //    }
        //    logger.Info($"The file path is successfully obtained. path : {result}");
        //    return result;
        //}

        //public static int GetThreadCountForUploadFiles()
        //{
        //    logger.Info($"The system starts to obtain the thread count for upload files.");
        //    int result = 0;
        //    UploadFileConfig uploadFileConfig = RuntimeContext.Config.UploadFileConfig;
        //    if (uploadFileConfig != null)
        //    {
        //        result = uploadFileConfig.ThreadCount;
        //    }
        //    logger.Info($"The thread count for upload files is successfully obtained. ThreadCount : {result}");
        //    return result;
        //}
        #endregion

        #region Output
        public static void OutputConfiguration()
        {
            OutputLoginConfiguration();
            //OutputOAuth2Configuration();
            OutputMicroServiceEndpointsConfiguration();
            //OutputMicroServiceEndpointsForGWConfiguration();
            //OutputCacheServerConfiguration();
            //OutputMessageBusConfiguration();
            //OutputTimerConfiguration();
            //OutputAuditServerConfiguration();
            //OutputAzureBlobConfiguration();
            //OutputGraphConfiguration();
            //OutputFileShareConfiguration();
            //OutputSurveyConfiguration();
            //OutputEmailConfiguration();
            //OutputAboutInfoConfiguration();
            //OutputIcareConfiguration();
            //OutputAllowAnonymousPathListConfiguration();
            //OutputSafeExamBrowerSettingsConfiguration();
        }




        public static void OutputDBConnectionConfiguration()
        {
            if (RuntimeContext.Config?.DBConnections != null)
            {
                logger.Info($"Init Service with DBConnections - DBType: {RuntimeContext.Config.DBConnections.DBType}");
                logger.Info($"Init Service with DBConnections - CoreDB: {RuntimeContext.Config.DBConnections.CoreDB}");
                logger.Info($"Init Service with DBConnections - AppDB: {RuntimeContext.Config.DBConnections.AppDB}");
                logger.Info($"Init Service with DBConnections - AuditDB: {RuntimeContext.Config.DBConnections.AuditDB}");
                logger.Info($"Init Service with DBConnections - CAPDB: {RuntimeContext.Config.DBConnections.CAPDB}");
                logger.Info($"Init Service with DBConnections - FADB: {RuntimeContext.Config.DBConnections.FADB}");
                logger.Info($"Init Service with DBConnections - PADB: {RuntimeContext.Config.DBConnections.PADB}");
                logger.Info($"Init Service with DBConnections - ELASTIC_POOL: {RuntimeContext.Config.DBConnections.ELASTIC_POOL}");
            }
            else
            {
                logger.Info($"Init Service with no DBConnections");
            }
        }

        public static void OutputLoginConfiguration()
        {
            logger.Info($"Init Service with LoginConfig - CookieDomain: {GetCookieDomain()}");
            logger.Info($"Init Service with LoginConfig - LoginCookieTimeOutInMinutes: {GetLoginCookieTimeOutInMinutes()}");
            logger.Info($"Init Service with LoginConfig - PermissionCookieTimeoutInMinutes: {GetPermissionCookieTimeoutInMinutes()}");
            logger.Info($"Init Service with LoginConfig - EnableSingleSessionLogin: {GetEnableSingleSessionLogin()}");
            logger.Info($"Init Service with LoginConfig - SingleSessionCookieTimeoutInSeconds: {GetSingleSessionCookieTimeoutInSeconds()}");
            logger.Info($"Init Service with LoginConfig - SingleSessionCacheTimeoutInMinutes: {GetSingleSessionCacheTimeoutInMinutes()}");
        }

        //public static void OutputOAuth2Configuration()
        //{
        //    if (RuntimeContext.Config?.OAuth2Configuration != null)
        //    {
        //        logger.Info($"Init Service with OAuth2Configuration - Authority: {GetAuthority()}");
        //        logger.Info($"Init Service with OAuth2Configuration - TenantId: {GetTenantId()}");
        //        logger.Info($"Init Service with OAuth2Configuration - ClientId: {GetClientId()}");
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no OAuth2Configuration");
        //    }
        //}

        public static void OutputMicroServiceEndpointsConfiguration()
        {
            if (RuntimeContext.Config?.MicroServiceEndpoints != null)
            {
                logger.Info($"Init Service with MicroServiceEndpoints - System: {RuntimeContext.Config.MicroServiceEndpoints.System}");
                logger.Info($"Init Service with MicroServiceEndpoints - WareHouse: {RuntimeContext.Config.MicroServiceEndpoints.WareHouse}");
                //logger.Info($"Init Service with MicroServiceEndpoints - Admin: {RuntimeContext.Config.MicroServiceEndpoints.Admin}");
                //logger.Info($"Init Service with MicroServiceEndpoints - ModuleMgt: {RuntimeContext.Config.MicroServiceEndpoints.ModuleMgt}");
                //logger.Info($"Init Service with MicroServiceEndpoints - ClassMgt: {RuntimeContext.Config.MicroServiceEndpoints.ClassMgt}");
                //logger.Info($"Init Service with MicroServiceEndpoints - LearningActivity: {RuntimeContext.Config.MicroServiceEndpoints.LearningActivity}");
                //logger.Info($"Init Service with MicroServiceEndpoints - Learning: {RuntimeContext.Config.MicroServiceEndpoints.Learning}");
                //logger.Info($"Init Service with MicroServiceEndpoints - IndustryStandard: {RuntimeContext.Config.MicroServiceEndpoints.IndustryStandard}");
                //logger.Info($"Init Service with MicroServiceEndpoints - Assessment: {RuntimeContext.Config.MicroServiceEndpoints.Assessment}");
                //logger.Info($"Init Service with MicroServiceEndpoints - Collaboration: {RuntimeContext.Config.MicroServiceEndpoints.Collaboration}");
                //logger.Info($"Init Service with MicroServiceEndpoints - Project: {RuntimeContext.Config.MicroServiceEndpoints.Project}");
                //logger.Info($"Init Service with MicroServiceEndpoints - Report: {RuntimeContext.Config.MicroServiceEndpoints.Report}");
                //logger.Info($"Init Service with MicroServiceEndpoints - Storage: {RuntimeContext.Config.MicroServiceEndpoints.Storage}");
                //logger.Info($"Init Service with MicroServiceEndpoints - Timer: {RuntimeContext.Config.MicroServiceEndpoints.Timer}");
                logger.Info($"Init Service with MicroServiceEndpoints - Identity: {RuntimeContext.Config.MicroServiceEndpoints.Identity}");
                //logger.Info($"Init Service with MicroServiceEndpoints - GuideHelp: {RuntimeContext.Config.MicroServiceEndpoints.GuideHelp}");
                //logger.Info($"Init Service with MicroServiceEndpoints - Portal: {RuntimeContext.Config.MicroServiceEndpoints.Portal}");
            }
            else
            {
                logger.Info($"Init Service with no MicroServiceEndpoints");
            }
        }

        //public static void OutputMicroServiceEndpointsForGWConfiguration()
        //{
        //    if (RuntimeContext.Config?.MicroServiceEndpointsForGW != null)
        //    {
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - Web: {RuntimeContext.Config.MicroServiceEndpointsForGW.Web}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - Common: {RuntimeContext.Config.MicroServiceEndpointsForGW.Common}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - Admin: {RuntimeContext.Config.MicroServiceEndpointsForGW.Admin}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - ModuleMgt: {RuntimeContext.Config.MicroServiceEndpointsForGW.ModuleMgt}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - ClassMgt: {RuntimeContext.Config.MicroServiceEndpointsForGW.ClassMgt}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - LearningActivity: {RuntimeContext.Config.MicroServiceEndpointsForGW.LearningActivity}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - Learning: {RuntimeContext.Config.MicroServiceEndpointsForGW.Learning}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - IndustryStandard: {RuntimeContext.Config.MicroServiceEndpointsForGW.IndustryStandard}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - Assessment: {RuntimeContext.Config.MicroServiceEndpointsForGW.Assessment}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - Collaboration: {RuntimeContext.Config.MicroServiceEndpointsForGW.Collaboration}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - Project: {RuntimeContext.Config.MicroServiceEndpointsForGW.Project}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - Report: {RuntimeContext.Config.MicroServiceEndpointsForGW.Report}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - Storage: {RuntimeContext.Config.MicroServiceEndpointsForGW.Storage}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - Timer: {RuntimeContext.Config.MicroServiceEndpointsForGW.Timer}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - Identity: {RuntimeContext.Config.MicroServiceEndpointsForGW.Identity}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - GuideHelp: {RuntimeContext.Config.MicroServiceEndpointsForGW.GuideHelp}");
        //        logger.Info($"Init Service with MicroServiceEndpointsForGW - Portal: {RuntimeContext.Config.MicroServiceEndpointsForGW.Portal}");
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no MicroServiceEndpointsForGW");
        //    }
        //}

        //public static void OutputCacheServerConfiguration()
        //{
        //    if (RuntimeContext.Config?.CacheServer != null)
        //    {
        //        logger.Info($"Init Service with CacheServer - CacheName: {RuntimeContext.Config.CacheServer.CacheName}");
        //        logger.Info($"Init Service with CacheServer - Password: {RuntimeContext.Config.CacheServer.Password}");
        //        logger.Info($"Init Service with CacheServer - RetryCount: {RuntimeContext.Config.CacheServer.RetryCount}");
        //        logger.Info($"Init Service with CacheServer - ReconnectInterval: {RuntimeContext.Config.CacheServer.ReconnectInterval}");
        //        logger.Info($"Init Service with CacheServer - ConnectionString: {RuntimeContext.Config.CacheServer.ConnectionString}");
        //        logger.Info($"Init Service with CacheServer - DefaultExpirationTime: {RuntimeContext.Config.CacheServer.DefaultExpirationTime}");
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no CacheServer");
        //    }
        //}

        //public static void OutputMessageBusConfiguration()
        //{
        //    if (RuntimeContext.Config?.MessageBusConfig != null)
        //    {
        //        logger.Info($"Init Service with MessageBusConfig - Type: {RuntimeContext.Config.MessageBusConfig.Type}");
        //        logger.Info($"Init Service with MessageBusConfig - Host: {RuntimeContext.Config.MessageBusConfig.Host}");
        //        logger.Info($"Init Service with MessageBusConfig - Port: {RuntimeContext.Config.MessageBusConfig.Port}");
        //        logger.Info($"Init Service with MessageBusConfig - ManagePort: {RuntimeContext.Config.MessageBusConfig.ManagePort}");
        //        logger.Info($"Init Service with MessageBusConfig - Username: {RuntimeContext.Config.MessageBusConfig.Username}");
        //        logger.Info($"Init Service with MessageBusConfig - Password: {RuntimeContext.Config.MessageBusConfig.Password}");
        //        logger.Info($"Init Service with MessageBusConfig - QueueName: {RuntimeContext.Config.MessageBusConfig.QueueName}");
        //        logger.Info($"Init Service with MessageBusConfig - VirtualHost: {RuntimeContext.Config.MessageBusConfig.VirtualHost}");
        //        logger.Info($"Init Service with MessageBusConfig - UseTls: {RuntimeContext.Config.MessageBusConfig.UseTls}");
        //        logger.Info($"Init Service with MessageBusConfig - RetryCount: {RuntimeContext.Config.MessageBusConfig.RetryCount}");
        //        logger.Info($"Init Service with MessageBusConfig - RetryIntervalSeconds: {RuntimeContext.Config.MessageBusConfig.RetryIntervalSeconds}");
        //        logger.Info($"Init Service with MessageBusConfig - ConsumerThreadCount: {RuntimeContext.Config.MessageBusConfig.ConsumerThreadCount}");
        //        logger.Info($"Init Service with MessageBusConfig - RetryPublishIntervalMinutes: {RuntimeContext.Config.MessageBusConfig.RetryPublishIntervalMinutes}");
        //        logger.Info($"Init Service with MessageBusConfig - NATSConnection: {RuntimeContext.Config.MessageBusConfig.NATSConnection}");
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no MessageBusConfig");
        //    }
        //}

        //public static void OutputTimerConfiguration()
        //{
        //    if (RuntimeContext.Config?.TimerConfig != null)
        //    {
        //        logger.Info($"Init Service with TimerConfig - InstanceName: {RuntimeContext.Config.TimerConfig.InstanceName}");
        //        logger.Info($"Init Service with TimerConfig - InstanceId: {RuntimeContext.Config.TimerConfig.InstanceId}");
        //        logger.Info($"Init Service with TimerConfig - BatchTriggerAcquisitionMaxCount: {RuntimeContext.Config.TimerConfig.BatchTriggerAcquisitionMaxCount}");
        //        logger.Info($"Init Service with TimerConfig - AcquireTriggersWithinLock: {RuntimeContext.Config.TimerConfig.AcquireTriggersWithinLock}");
        //        logger.Info($"Init Service with TimerConfig - SerializerType: {RuntimeContext.Config.TimerConfig.SerializerType}");
        //        logger.Info($"Init Service with TimerConfig - MaxConcurrency: {RuntimeContext.Config.TimerConfig.MaxConcurrency}");
        //        logger.Info($"Init Service with TimerConfig - RetryCount: {RuntimeContext.Config.TimerConfig.RetryCount}");
        //        logger.Info($"Init Service with TimerConfig - RetryIntervalSeconds: {RuntimeContext.Config.TimerConfig.RetryIntervalSeconds}");
        //        logger.Info($"Init Service with TimerConfig - Clustered: {RuntimeContext.Config.TimerConfig.Clustered}");
        //        logger.Info($"Init Service with TimerConfig - JobStoreType: {RuntimeContext.Config.TimerConfig.JobStoreType}");
        //        logger.Info($"Init Service with TimerConfig - DriverDelegateType: {RuntimeContext.Config.TimerConfig.DriverDelegateType}");
        //        logger.Info($"Init Service with TimerConfig - TablePrefix: {RuntimeContext.Config.TimerConfig.TablePrefix}");
        //        logger.Info($"Init Service with TimerConfig - DataSource: {RuntimeContext.Config.TimerConfig.DataSource}");
        //        logger.Info($"Init Service with TimerConfig - ConnectionString: {RuntimeContext.Config.TimerConfig.ConnectionString}");
        //        logger.Info($"Init Service with TimerConfig - Provider: {RuntimeContext.Config.TimerConfig.Provider}");
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no TimerConfig");
        //    }
        //}

        //public static void OutputAuditServerConfiguration()
        //{
        //    if (RuntimeContext.Config?.AuditServer != null)
        //    {
        //        logger.Info($"Init Service with AuditServer - AuditTransmission: {RuntimeContext.Config.AuditServer.AuditTransmission}");
        //        logger.Info($"Init Service with AuditServer - UseModuleDB: {RuntimeContext.Config.AuditServer.UseModuleDB}");
        //        logger.Info($"Init Service with AuditServer - AuditTableFormat: {RuntimeContext.Config.AuditServer.AuditTableFormat}");
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no AuditServer");
        //    }
        //}

        //public static void OutputAzureBlobConfiguration()
        //{
        //    if (RuntimeContext.Config?.AzureBlobConfig != null)
        //    {
        //        logger.Info($"Init Service with AzureBlobConfig - AccessPoint: {RuntimeContext.Config.AzureBlobConfig.AccessPoint}");
        //        logger.Info($"Init Service with AzureBlobConfig - AccountName: {RuntimeContext.Config.AzureBlobConfig.AccountName}");
        //        logger.Info($"Init Service with AzureBlobConfig - AccountKey: {RuntimeContext.Config.AzureBlobConfig.AccountKey}");
        //        logger.Info($"Init Service with AzureBlobConfig - Container: {RuntimeContext.Config.AzureBlobConfig.Container}");
        //        logger.Info($"Init Service with AzureBlobConfig - TempContainer: {RuntimeContext.Config.AzureBlobConfig.TempContainer}");
        //        logger.Info($"Init Service with AzureBlobConfig - StorageRoot: {RuntimeContext.Config.AzureBlobConfig.StorageRoot}");
        //        logger.Info($"Init Service with AzureBlobConfig - TempRoot: {RuntimeContext.Config.AzureBlobConfig.TempRoot}");
        //        logger.Info($"Init Service with AzureBlobConfig - ClientId: {RuntimeContext.Config.AzureBlobConfig.ClientId}");
        //        logger.Info($"Init Service with AzureBlobConfig - TenantId: {RuntimeContext.Config.AzureBlobConfig.TenantId}");
        //        logger.Info($"Init Service with AzureBlobConfig - Secret: {RuntimeContext.Config.AzureBlobConfig.Secret}");
        //        logger.Info($"Init Service with AzureBlobConfig - IsDefault: {RuntimeContext.Config.AzureBlobConfig.IsDefault}");
        //        logger.Info($"Init Service with AzureBlobConfig - TempFilePath: {RuntimeContext.Config.AzureBlobConfig.TempFilePath}");
        //        logger.Info($"Init Service with AzureBlobConfig - TempSPOnlineSite: {RuntimeContext.Config.AzureBlobConfig.TempSPOnlineSite}");
        //        logger.Info($"Init Service with AzureBlobConfig - Prefix: {RuntimeContext.Config.AzureBlobConfig.Prefix}");
        //        logger.Info($"Init Service with AzureBlobConfig - UserName: {RuntimeContext.Config.AzureBlobConfig.UserName}");
        //        logger.Info($"Init Service with AzureBlobConfig - Password: {RuntimeContext.Config.AzureBlobConfig.Password}");
        //        logger.Info($"Init Service with AzureBlobConfig - DefaultAADAppId: {RuntimeContext.Config.AzureBlobConfig.DefaultAADAppId}");
        //        logger.Info($"Init Service with AzureBlobConfig - DefaultModuleSite: {RuntimeContext.Config.AzureBlobConfig.DefaultModuleSite}");
        //        logger.Info($"Init Service with AzureBlobConfig - PlaceholderSite: {RuntimeContext.Config.AzureBlobConfig.PlaceholderSite}");
        //        logger.Info($"Init Service with AzureBlobConfig - DefaultPersonalSite: {RuntimeContext.Config.AzureBlobConfig.DefaultPersonalSite}");
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no AzureBlobConfig");
        //    }
        //}

        //public static void OutputGraphConfiguration()
        //{
        //    if (RuntimeContext.Config?.GraphConfig != null)
        //    {
        //        logger.Info($"Init Service with GraphConfig - TenantId: {RuntimeContext.Config.GraphConfig.TenantId}");
        //        logger.Info($"Init Service with GraphConfig - ClientId: {RuntimeContext.Config.GraphConfig.ClientId}");
        //        logger.Info($"Init Service with GraphConfig - ClientSecret: {RuntimeContext.Config.GraphConfig.ClientSecret}");
        //        logger.Info($"Init Service with GraphConfig - Username: {RuntimeContext.Config.GraphConfig.Username}");
        //        logger.Info($"Init Service with GraphConfig - Password: {RuntimeContext.Config.GraphConfig.Password}");
        //        logger.Info($"Init Service with GraphConfig - Sender: {RuntimeContext.Config.GraphConfig.Sender}");
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no GraphConfig");
        //    }
        //}

        //public static void OutputFileShareConfiguration()
        //{
        //    if (RuntimeContext.Config?.FileShareConfig != null)
        //    {
        //        logger.Info($"Init Service with FileShareConfig - TempFilePath: {RuntimeContext.Config.FileShareConfig.TempFilePath}");
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no FileShareConfig");
        //    }
        //}

        //public static void OutputSurveyConfiguration()
        //{
        //    if (RuntimeContext.Config?.SurveyConfig != null)
        //    {
        //        logger.Info($"Init Service with SurveyConfig - SSInternalStaffEmail: {RuntimeContext.Config.SurveyConfig.SSInternalStaffEmail}");
        //        logger.Info($"Init Service with SurveyConfig - SSInternalStaffEmail2: {RuntimeContext.Config.SurveyConfig.SSInternalStaffEmail2}");
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no SurveyConfig");
        //    }
        //}

        //public static void OutputEmailConfiguration()
        //{
        //    if (RuntimeContext.Config?.EmailConfig != null)
        //    {
        //        logger.Info($"Init Service with EmailConfig - PerEmailUserCount: {RuntimeContext.Config.EmailConfig.PerEmailUserCount}");
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no EmailConfig");
        //    }
        //}

        //public static void OutputAboutInfoConfiguration()
        //{
        //    if (RuntimeContext.Config?.AboutInfo != null)
        //    {
        //        logger.Info($"Init Service with AboutInfo - Version: {RuntimeContext.Config.AboutInfo.Version}");
        //        logger.Info($"Init Service with AboutInfo - UpdatedTime: {RuntimeContext.Config.AboutInfo.UpdatedTime}");
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no AboutInfo");
        //    }
        //}

        //public static void OutputIcareConfiguration()
        //{
        //    if (RuntimeContext.Config?.IcareConfig != null)
        //    {
        //        logger.Info($"Init Service with IcareConfig - Host: {RuntimeContext.Config.IcareConfig.Host}");
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no IcareConfig");
        //    }
        //}

        //public static void OutputAllowAnonymousPathListConfiguration()
        //{
        //    if (RuntimeContext.Config?.AllowAnonymousPathList != null && RuntimeContext.Config?.AllowAnonymousPathList.Count > 0)
        //    {
        //        logger.Info($"Init Service with AllowAnonymousPathList: {String.Join(";", RuntimeContext.Config.AllowAnonymousPathList.ToArray())}");
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no AllowAnonymousPathList");
        //    }
        //}

        //public static void OutputSafeExamBrowerSettingsConfiguration()
        //{
        //    if (RuntimeContext.Config?.SafeExamBrowerSettings != null && RuntimeContext.Config?.SafeExamBrowerSettings.Count > 0)
        //    {
        //        foreach (SEBTemplateSettings oneSetting in RuntimeContext.Config?.SafeExamBrowerSettings)
        //        {
        //            OutputOneSafeExamBrowerSettings(oneSetting);
        //        }
        //    }
        //    else
        //    {
        //        logger.Info($"Init Service with no SafeExamBrowerSettings");
        //    }
        //}

        //private static void OutputOneSafeExamBrowerSettings(SEBTemplateSettings setting)
        //{
        //    logger.Info($"Init Service with SEBTemplateSettings - Id: {setting.Id}");
        //    logger.Info($"Init Service with SEBTemplateSettings - SEBTemplateName: {setting.SEBTemplateName}");
        //    logger.Info($"Init Service with SEBTemplateSettings - Description: {setting.Description}");
        //    logger.Info($"Init Service with SEBTemplateSettings - WindowsSEBFileId: {setting.WindowsSEBFileId}");
        //    logger.Info($"Init Service with SEBTemplateSettings - MACSEBFileId: {setting.MACSEBFileId}");
        //    logger.Info($"Init Service with SEBTemplateSettings - WinSEBFileName: {setting.WinSEBFileName}");
        //    logger.Info($"Init Service with SEBTemplateSettings - MACSEBFileName: {setting.MACSEBFileName}");
        //    logger.Info($"Init Service with SEBTemplateSettings - WindowsKey: {setting.WindowsKey}");
        //    logger.Info($"Init Service with SEBTemplateSettings - MACKey: {setting.MACKey}");
        //    logger.Info($"Init Service with SEBTemplateSettings - IsBuildIn: {setting.IsBuildIn}");
        //    logger.Info($"Init Service with SEBTemplateSettings - CreateTime: {setting.CreateTime}");
        //    logger.Info($"Init Service with SEBTemplateSettings - IsChecked: {setting.IsChecked}");
        //}
        #endregion
    }
}
