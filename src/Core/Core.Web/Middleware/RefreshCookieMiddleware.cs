//using Core.Domain;
//using Core.Utility;
//using Microsoft.AspNetCore.Authentication;
//using System.Security.Claims;

//namespace Core.Web
//{
//    public class RefreshCookieMiddleware
//    {
//        private static AveLogger logger = AveLogger.GetInstance(typeof(RefreshCookieMiddleware));

//        private readonly RequestDelegate _next;


//        public RefreshCookieMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task Invoke(HttpContext context, ILEOLoginURLService leoLoginURLService)
//        {
//            if (!string.IsNullOrEmpty(context.Request.ContentType) && context.Request.ContentType.Equals(@"application/grpc"))
//            {
//                //Skip GRPC Request. Do not add any role information into httpContext.
//                await _next(context);
//            }
//            else
//            {
//                if (context.User.Identity.IsAuthenticated)
//                {
//                    bool skipCheck = false;

//                    #region Exclude
//                    //Skip JWT Authorization
//                    string text = context.Request?.Headers["Authorization"];
//                    if (!string.IsNullOrEmpty(text) && text.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
//                    {
//                        skipCheck = true;
//                    }

//                    //Skip exclude url
//                    if (context.Request.Path.Value.Equals("/") || context.Request.Path.Value.Equals("/.") ||
//                        context.Request.Path.Value.StartsWith("/lib/", StringComparison.OrdinalIgnoreCase) ||
//                        context.Request.Path.Value.Contains("/login", StringComparison.OrdinalIgnoreCase) ||
//                        context.Request.Path.Value.Contains("/identity/login", StringComparison.OrdinalIgnoreCase) ||
//                        context.Request.Path.Value.Contains("/identity/logincallback", StringComparison.OrdinalIgnoreCase) ||
//                        context.Request.Path.Value.Contains("/identity/logout", StringComparison.OrdinalIgnoreCase) ||
//                        context.Request.Path.Value.Contains("/identity/logoutcallback", StringComparison.OrdinalIgnoreCase) ||
//                        context.Request.Path.Value.Contains("/identity/oidcsignout", StringComparison.OrdinalIgnoreCase) ||
//                        context.Request.Path.Value.Contains("/identity/concurrentlogin", StringComparison.OrdinalIgnoreCase) ||
//                        context.Request.Path.Value.Contains("/identity/concurrentlogout", StringComparison.OrdinalIgnoreCase))
//                    {
//                        skipCheck = true;
//                    }
//                    #endregion

//                    if (!skipCheck)
//                    {
//                        bool skipRefreshExpiresTime = false;
//                        if (context.Request.Path.Value.Contains("/common/gettimeoutleft", StringComparison.OrdinalIgnoreCase))
//                        {
//                            skipRefreshExpiresTime = true;
//                        }

//                        if (!skipRefreshExpiresTime)
//                        {
//                            #region Check Concurrent/Single login session
//                            bool needRefreshSecuredSessionCacheDateTime = false;
//                            DateTime refreshSecuredSessionCacheDateTime = DateTime.UtcNow;
//                            if (ConfigurationHelper.GetEnableSingleSessionLogin())
//                            {
//                                bool needCheckConcurrentStatus = false;
//                                var existConcurrentResult = await context.AuthenticateAsync(LEOAuthenticationConstants.SingleSessionCookieName);
//                                if (existConcurrentResult != null && existConcurrentResult.Succeeded)
//                                {
//                                    var existConcurrentProps = existConcurrentResult.Properties;
//                                    if (existConcurrentProps.ExpiresUtc < DateTimeOffset.UtcNow)
//                                    {
//                                        needCheckConcurrentStatus = true;
//                                    }
//                                    else
//                                    {
//                                        //Cookie is available, skip Concurrent check
//                                    }
//                                }
//                                else
//                                {
//                                    needCheckConcurrentStatus = true;
//                                }

//                                if (needCheckConcurrentStatus)
//                                {
//                                    Guid userGuid = context.GetUserId();
//                                    if (userGuid != Guid.Empty)
//                                    {
//                                        bool needAddSecuredSessionCache = false;

//                                        string userId = userGuid.ToString();
//                                        string userHost = await leoLoginURLService.GetHostURLAsync();
//                                        string userSessionCacheKey = string.Format(CookieConstants.SingleSessionKey, userId, userHost);
//                                        string userSecuredSessionID = context.GetSecuredSessionId();
//                                        string realUserSecuredSessionID = AesEncryptionHelper.DecryptString(userSecuredSessionID);

//                                        string sessionString = null;
//                                        bool fetchStatus = CacheHelper.FetchItem<string>(userSessionCacheKey, out sessionString);
//                                        if (fetchStatus && !string.IsNullOrEmpty(sessionString))
//                                        {
//                                            string realSessionString = AesEncryptionHelper.DecryptString(sessionString);
//                                            if (!realSessionString.Equals(realUserSecuredSessionID))
//                                            {
//                                                logger.Warn("**** Concurrent Unauthorized ****");
//                                                throw new ConcurrentUnauthorizedException("ConcurrentUnauthorized");
//                                            }
//                                            else
//                                            {
//                                                //check pass
//                                            }
//                                        }
//                                        else
//                                        {
//                                            needAddSecuredSessionCache = true;
//                                        }

//                                        if (needAddSecuredSessionCache)
//                                        {
//                                            CacheHelper.CacheItem(userSessionCacheKey, userSecuredSessionID, new TimeSpan(0, ConfigurationHelper.GetSingleSessionCacheTimeoutInMinutes(), 0));
//                                        }
//                                        else
//                                        {
//                                            DateTime sessionExpiresUtcDateTime = context.GetSecuredSessionCacheExpiresUtcDateTime();
//                                            DateTime calculatorUtcDateTime = DateTime.UtcNow.AddMinutes(ConfigurationHelper.GetSingleSessionCacheTimeoutInMinutes() / 2);
//                                            if (sessionExpiresUtcDateTime < calculatorUtcDateTime)
//                                            {
//                                                CacheHelper.CacheItem(userSessionCacheKey, userSecuredSessionID, new TimeSpan(0, ConfigurationHelper.GetSingleSessionCacheTimeoutInMinutes(), 0));
//                                                needRefreshSecuredSessionCacheDateTime = true;
//                                                refreshSecuredSessionCacheDateTime = DateTime.UtcNow.AddMinutes(ConfigurationHelper.GetSingleSessionCacheTimeoutInMinutes());
//                                            }
//                                        }

//                                        var userConcurrentIdentity = new ClaimsIdentity(LEOAuthenticationConstants.SingleSessionCookieName);
//                                        ClaimsPrincipal userConcurrentPrincipal = new ClaimsPrincipal(userConcurrentIdentity);
//                                        var userConcurrentProperties = new AuthenticationProperties();
//                                        userConcurrentProperties.IsPersistent = false;
//                                        userConcurrentProperties.IssuedUtc = DateTimeOffset.UtcNow;
//                                        userConcurrentProperties.ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(ConfigurationHelper.GetSingleSessionCookieTimeoutInSeconds());
//                                        userConcurrentProperties.AllowRefresh = false;
//                                        context.SignInAsync(LEOAuthenticationConstants.SingleSessionCookieName, userConcurrentPrincipal, userConcurrentProperties).Wait();
//                                    }
//                                }
//                            }
//                            #endregion

//                            #region Refresh ExpiresTime
//                            try
//                            {
//                                #region Refresh Login ExpiresTime
//                                DateTimeOffset refreshLoginExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(ConfigurationHelper.GetLoginCookieTimeOutInMinutes());
//                                var existLoginResult = await context.AuthenticateAsync(LEOAuthenticationConstants.LoginCookieName);
//                                if (existLoginResult != null && existLoginResult.Succeeded)
//                                {
//                                    var existLoginProps = existLoginResult.Properties;
//                                    if (existLoginProps.ExpiresUtc < refreshLoginExpiresUtc)
//                                    {
//                                        var updateUserLoginIdentity = new ClaimsIdentity(LEOAuthenticationConstants.LoginCookieName);
//                                        if (existLoginResult.Principal.Claims != null)
//                                        {
//                                            foreach (Claim c in existLoginResult.Principal.Claims)
//                                            {
//                                                if (c.Type.Equals(ClaimConstants.SessionExpiresUtcTicks, StringComparison.OrdinalIgnoreCase))
//                                                {
//                                                    updateUserLoginIdentity.AddClaim(new Claim(ClaimConstants.SessionExpiresUtcTicks, refreshLoginExpiresUtc.Ticks.ToString()));
//                                                }
//                                                else if (c.Type.Equals(ClaimConstants.SecuredSessionCacheExpiresUtcTicks, StringComparison.OrdinalIgnoreCase))
//                                                {
//                                                    if (needRefreshSecuredSessionCacheDateTime)
//                                                    {
//                                                        updateUserLoginIdentity.AddClaim(new Claim(ClaimConstants.SecuredSessionCacheExpiresUtcTicks, refreshSecuredSessionCacheDateTime.Ticks.ToString()));
//                                                    }
//                                                    else
//                                                    {
//                                                        updateUserLoginIdentity.AddClaim(c);
//                                                    }
//                                                }
//                                                else if (c.Type.Equals(ClaimConstants.BaseIdentity, StringComparison.OrdinalIgnoreCase))
//                                                {
//                                                    continue;
//                                                }
//                                                else
//                                                {
//                                                    updateUserLoginIdentity.AddClaim(c);
//                                                }
//                                            }
//                                        }
//                                        ClaimsPrincipal updateUserLoginPrincipal = new ClaimsPrincipal(updateUserLoginIdentity);
//                                        var newUserLoginProperties = new AuthenticationProperties();
//                                        newUserLoginProperties.IsPersistent = false;
//                                        newUserLoginProperties.IssuedUtc = existLoginProps.IssuedUtc;
//                                        newUserLoginProperties.ExpiresUtc = refreshLoginExpiresUtc;
//                                        newUserLoginProperties.AllowRefresh = false;
//                                        context.SignInAsync(LEOAuthenticationConstants.LoginCookieName, updateUserLoginPrincipal, newUserLoginProperties).Wait();
//                                    }
//                                }
//                                #endregion

//                                #region Refresh Permission ExpiresTime
//                                DateTimeOffset refreshPermissionExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(ConfigurationHelper.GetPermissionCookieTimeoutInMinutes());
//                                var existPermissionResult = await context.AuthenticateAsync(LEOAuthenticationConstants.PermissionCookieName);
//                                if (existPermissionResult != null && existPermissionResult.Succeeded)
//                                {
//                                    var existPermissionProps = existPermissionResult.Properties;
//                                    if (existPermissionProps.ExpiresUtc < refreshPermissionExpiresUtc)
//                                    {
//                                        var updateUserPermissionIdentity = new ClaimsIdentity(LEOAuthenticationConstants.PermissionCookieName);
//                                        if (existPermissionResult.Principal.Claims != null)
//                                        {
//                                            foreach (Claim c in existPermissionResult.Principal.Claims)
//                                            {
//                                                if (c.Type.Equals(ClaimConstants.BaseIdentity, StringComparison.OrdinalIgnoreCase))
//                                                {
//                                                    updateUserPermissionIdentity.AddClaim(c);
//                                                }
//                                            }
//                                        }
//                                        ClaimsPrincipal updateUserPermissionPrincipal = new ClaimsPrincipal(updateUserPermissionIdentity);
//                                        var newUserPermissionProperties = new AuthenticationProperties();
//                                        newUserPermissionProperties.IsPersistent = false;
//                                        newUserPermissionProperties.IssuedUtc = existPermissionProps.IssuedUtc;
//                                        newUserPermissionProperties.ExpiresUtc = refreshPermissionExpiresUtc;
//                                        newUserPermissionProperties.AllowRefresh = false;
//                                        context.SignInAsync(LEOAuthenticationConstants.PermissionCookieName, updateUserPermissionPrincipal, newUserPermissionProperties).Wait();
//                                    }
//                                }
//                                #endregion
//                            }
//                            catch (Exception ex)
//                            {
//                                logger.Warn("An error occurred while refresh cookie.", ex);
//                            }
//                            #endregion
//                        }
//                    }
//                }
//                await _next(context);
//            }
//        }
//    }
//}
