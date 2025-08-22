using Core.Domain;
using Core.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System.Net;
using System.Security.Claims;

namespace Core.Web
{
    public static class HttpContextExtensions
    {
        private static readonly SiasunLogger logger = SiasunLogger.GetInstance(typeof(HttpContextExtensions));

        public static BaseIdentity GetBaseIdentity(this HttpContext httpContext)
        {
            BaseIdentity baseIdentity = null;
            if (httpContext != null && httpContext.User != null)
            {
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    //var existPermissionResult = httpContext.AuthenticateAsync(LEOAuthenticationConstants.PermissionCookieName).Result;
                    //if (existPermissionResult != null && existPermissionResult.Succeeded)
                    //{
                    //    var baseIdentityClaim = existPermissionResult.Principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimConstants.BaseIdentity));
                    //    if (baseIdentityClaim != null)
                    //    {
                    //        baseIdentity = JsonConvert.DeserializeObject<BaseIdentity>(baseIdentityClaim.Value);
                    //    }
                    //}
                    //if (baseIdentity != null)
                    //{
                    //    var existLoginResult = httpContext.AuthenticateAsync(LEOAuthenticationConstants.LoginCookieName).Result;
                    //    if (existLoginResult != null && existLoginResult.Succeeded)
                    //    {
                    //        var sessionExpiresUtcTicksClaim = existLoginResult.Principal.Claims.FirstOrDefault(c => c.Type.Equals(ClaimConstants.SessionExpiresUtcTicks));
                    //        if (sessionExpiresUtcTicksClaim != null)
                    //        {
                    //            baseIdentity.ExpiredTime = new DateTime(long.Parse(sessionExpiresUtcTicksClaim.Value));
                    //        }
                    //    }
                    //}
                    #region For JWT Authentication
                    if (baseIdentity == null)
                    {
                        var jwtAuthResult = httpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme).Result;
                        if (jwtAuthResult != null && jwtAuthResult.Succeeded)
                        {
                            baseIdentity = httpContext.GenerateCurrentUserForJwt();
                        }
                    }
                    #endregion
                }
            }
            return baseIdentity;
        }
        public static BaseIdentity GenerateCurrentUserForJwt(this HttpContext context)
        {
            BaseIdentity baseIdentity = null;
            //var leoAuthorizationService = context.RequestServices.GetService(typeof(ILEOAuthorizationService)) as ILEOAuthorizationService;
            var claim = context.User.Identity as ClaimsIdentity;
            if (claim != null && claim.Claims.Count() > 0)
            {
                var key = claim.Name;
                //var scopes = claim.Claims.Where(c => c.Type.Equals(LEOOAuth2Constants.OAuth2_Claim_Scope, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(c.Value)).Select(c => c.Value).ToList();
                //var grantType = claim.Claims.Where(c => string.Equals(c.Type, LEOOAuth2Constants.OAuth2_Claim_GrantType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                //if (grantType != null && string.Equals(grantType.Value, GrantTypes.Client.ToString(), StringComparison.OrdinalIgnoreCase))
                //{
                //    Credential authCredential = new LocalCredential();
                //    LocalCredential localCredential = authCredential as LocalCredential;
                //    localCredential.Username = IdentityUtil.SystemAccount;
                //    baseIdentity = leoAuthorizationService.Authorization(authCredential);
                //    baseIdentity.AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;
                //}
                //else if (grantType != null && string.Equals(grantType.Value, GrantTypes.Code.ToString(), StringComparison.OrdinalIgnoreCase))
                //{
                //    Credential credential = new AADCredential();
                //    AADCredential aadCredential = credential as AADCredential;
                //    aadCredential.UPN = context.GetLoginUPN();
                //    baseIdentity = leoAuthorizationService.Authorization(credential);
                //    baseIdentity.AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme;
                //}
            }
            return baseIdentity;
        }

        public static AccountIdentity GetCurrentUser(this HttpContext httpContext)
        {
            var baseIdentity = httpContext.GetBaseIdentity();
            if (baseIdentity != null && !string.IsNullOrEmpty(baseIdentity.AuthenticationScheme)
                && baseIdentity.AuthenticationScheme.Equals(JwtBearerDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
            {
                return httpContext.GetWebAPIIdentity(baseIdentity);
            }
            else
            {
                return new AccountIdentity(baseIdentity);
            }
        }

        public static WebAPIAccountIdentity GetWebAPIIdentity(this HttpContext context, BaseIdentity baseIdentity)
        {
            var claim = context.User.Identity as ClaimsIdentity;
            if (claim != null && claim.Claims.Count() > 0)
            {
                var key = claim.Name;
                //var scopes = claim.Claims.Where(c => c.Type.Equals(LEOOAuth2Constants.OAuth2_Claim_Scope, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(c.Value)).Select(c => c.Value).ToList();
                //var grantType = claim.Claims.Where(c => string.Equals(c.Type, LEOOAuth2Constants.OAuth2_Claim_GrantType, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                var scopes = claim.Claims.Where(c => !string.IsNullOrEmpty(c.Value)).Select(c => c.Value).ToList();
                var grantType = claim.Claims.FirstOrDefault();
                
                return new WebAPIAccountIdentity(scopes) { Identity = baseIdentity, APIName = claim.Name, APIMode = grantType.Value };
            }
            else
            {
                return new WebAPIAccountIdentity(new List<string>()) { Identity = baseIdentity };
            }
        }

        public static string GetUsername(this HttpContext httpContext)
        {
            return httpContext.GetBaseIdentity()?.LoginName;
        }

        public static string GetUPN(this HttpContext httpContext)
        {
            return httpContext.GetBaseIdentity()?.UPN;
        }

        public static Guid GetAADObjectId(this HttpContext httpContext)
        {
            string idString = httpContext.GetBaseIdentity()?.AADObjectId;
            Guid aadObjectId = Guid.Empty;
            if (!string.IsNullOrEmpty(idString))
            {
                Guid.TryParse(idString, out aadObjectId);
            }
            return aadObjectId;
        }

        public static Guid GetUserId(this HttpContext httpContext)
        {
            BaseIdentity identity = httpContext.GetBaseIdentity();
            if (identity != null)
            {
                return identity.ID;
            }
            else
            {
                return Guid.Empty;
            }
        }

        public static string GetUserDisplayName(this HttpContext httpContext)
        {
            return httpContext.GetBaseIdentity()?.DisplayName;
        }

        public static UserType GetUserAccountType(this HttpContext httpContext)
        {
            BaseIdentity identity = httpContext.GetBaseIdentity();
            if (identity != null)
            {
                return identity.AccountType;
            }
            else
            {
                return UserType.Invalid;
            }
        }

        public static string GetClientIpAddress(this HttpContext httpContext)
        {
            try
            {
                #region New Method
                string ip = GetHeaderValueAs(httpContext, "X-Forwarded-For")?.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

                // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
                if (string.IsNullOrWhiteSpace(ip) && httpContext?.Connection?.RemoteIpAddress != null)
                {
                    ip = httpContext.Connection.RemoteIpAddress.ToString();
                }

                if (string.IsNullOrWhiteSpace(ip))
                {
                    ip = GetHeaderValueAs(httpContext, "REMOTE_ADDR");
                }

                if (RuntimeContext.Config.IsDevelopment)
                {
                    if (string.IsNullOrWhiteSpace(ip) || ip.ToLower().Equals("127.0.0.1"))
                    {
                        ip = NetWork.LocalIPAddress;
                    }
                }

                logger.Debug($"GetClientIpAddress use new Method:{ip?.Trim()}");
                return ip;
                #endregion

                #region Old Method
                //var currentRequest = httpContext.Request;
                //string customerIp = string.Empty;
                //if (currentRequest != null)
                //{
                //    if (currentRequest.Headers["VIA"].FirstOrDefault() != null)
                //    {
                //        //获得位于代理(网关)后面的直接IP
                //        customerIp = currentRequest.Headers["X-Forwarded-For"];
                //    }
                //    if (string.IsNullOrEmpty(customerIp))
                //    {
                //        //发出请求的远程主机的IP地址
                //        customerIp = currentRequest.Headers["REMOTE_ADDR"];
                //    }
                //    if (string.IsNullOrEmpty(customerIp))
                //    {
                //        customerIp = httpContext.Connection.RemoteIpAddress.ToString();
                //    }
                //    if (customerIp == "::1")
                //    {
                //        //ipv4 localhost 或者 127.0.0.1
                //        customerIp = Environment.MachineName;
                //    }
                //}
                //logger.Info($"GetClientIpAddress use old Method:{customerIp}");
                //return customerIp;
                #endregion
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetClientBrowser(this HttpContext httpContext)
        {
            try
            {
                string browser = GetHeaderValueAs(httpContext, "User-Agent");
                browser = browser.Replace(" ", "");
                browser = browser.TrimStart().TrimEnd();

                logger.Debug($"GetClientBrowser:{browser}");
                return browser;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string GetHeaderValueAs(HttpContext httpContext, string headerName)
        {
            StringValues values;

            if (httpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();

                if (!string.IsNullOrWhiteSpace(rawValues))
                    return rawValues;
            }
            return string.Empty;
        }

        public static string GetSecuredSessionId(this HttpContext httpContext, string userId, string sid)
        {
            string sessionCharacteristicsString = string.Empty;
            if (!string.IsNullOrEmpty(sid))
            {
                sessionCharacteristicsString = string.Format("{0}-{1}", userId, sid);
            }
            else
            {
                //sessionCharacteristicsString = string.Format("{0}-{1}", userId, DateTime.UtcNow.Ticks.ToString()
                sessionCharacteristicsString = string.Format("{0}-{1}-{2}", userId, httpContext.GetClientIpAddress(), httpContext.GetClientBrowser());
            }
            logger.Debug($"Get SessionCharacteristicsString:{sessionCharacteristicsString}");
            string securedSessionId = string.Empty;
            if (!string.IsNullOrEmpty(sessionCharacteristicsString))
            {
                securedSessionId = AesEncryptionHelper.EncryptString(sessionCharacteristicsString);
            }
            return securedSessionId;
        }
    }
}
