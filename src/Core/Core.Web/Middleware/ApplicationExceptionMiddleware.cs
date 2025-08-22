using Core.Domain;
using Core.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace Core.Web
{
    public class ApplicationExceptionMiddleware
    {
        private static SiasunLogger logger = SiasunLogger.GetInstance(typeof(ApplicationExceptionMiddleware));

        private readonly RequestDelegate _next;

        public ApplicationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await ErrorHandlingAsync(ex, context);
            }
        }

        public async Task ErrorHandlingAsync(Exception ex, HttpContext context)
        {
            int httpCode = 0;
            string redirectUrl = string.Empty;
            HttpStatusCode statusCode = HttpStatusCode.OK;
            var httpException = ex as HttpException;
            if (httpException == null)
            {
                if (ex is LoginUnauthorizedException
                    || ex is ConcurrentUnauthorizedException)
                {
                    statusCode = HttpStatusCode.Unauthorized;
                    httpCode = (int)statusCode;
                }
                else if (ex is AccessDeniedException
                    || ex is HttpUnhandledException)
                {
                    statusCode = HttpStatusCode.MethodNotAllowed;
                    httpCode = (int)statusCode;
                }
                else
                {
                    statusCode = HttpStatusCode.Unused;
                    httpCode = (int)statusCode;
                }
            }
            else
            {
                httpCode = httpException.StatusCode;
                statusCode = (HttpStatusCode)httpCode;
            }
            if (httpException != null && httpException.StatusCode == 401)
            {
                logger.Error($"**** Request Unauthorized, URL: {context.Request.AbsoluteUri()} ****");
            }
            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    redirectUrl = string.Format("{0}?error={1}", "/Corebase/Error/Index", httpCode);
                    logger.Error($"Exception: 401 Unauthorized redirectUrl:{redirectUrl}");
                    break;
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.ServiceUnavailable:
                    redirectUrl = string.Format("{0}?error={1}", "/Corebase/Error/Index", httpCode);
                    logger.Error($"Exception: Code:{httpCode} redirectUrl:{redirectUrl}");
                    if (context.Request.Query["X-Requested-With"] == "XMLHttpRequest" || context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        ProcessAjaxRequest(context, httpCode, ex.Message, redirectUrl);
                        return;
                    }
                    break;
                case HttpStatusCode.MethodNotAllowed:
                    int subCode = (int)ExceptionMessage.CF_NoAuthorize_NoPermission_Message;
                    //if (ex.Message.Equals(ExceptionMessage.CF_NoAuthorize_Simulation_Message.ToDescription()))
                    //{
                    //    subCode = (int)ExceptionMessage.CF_NoAuthorize_Simulation_Message;
                    //}
                    //else if (ex.Message.Equals(ExceptionMessage.CF_NoAuthorize_DraftModule_Message.ToDescription()))
                    //{
                    //    subCode = (int)ExceptionMessage.CF_NoAuthorize_DraftModule_Message;
                    //}
                    redirectUrl = string.Format("{0}?error={1}.{2}", "/Corebase/Error/NoAuthorize", httpCode, subCode);
                    redirectUrl = redirectUrl + "&request=" + HttpUtility.UrlEncode(context.Request?.Path.Value ?? string.Empty);
                    logger.Error($"Exception: 405 MethodNotAllowed redirectUrl:{redirectUrl}");
                    if (context.Request.Query["X-Requested-With"] == "XMLHttpRequest" || context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        ProcessAjaxRequest(context, httpCode, ex.Message, redirectUrl);
                        return;
                    }
                    break;
                case HttpStatusCode.Unused:
                    logger.Error($"Exception: 306 Unused redirectUrl:{redirectUrl}");
                    if (context.Request.Query["X-Requested-With"] == "XMLHttpRequest" || context.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        ex = new UnExpectedException();
                        ProcessAjaxRequest(context, httpCode, ex.Message, redirectUrl);
                        return;
                    }
                    break;
                default:
                    logger.Error($"Exception: Others redirectUrl:{redirectUrl}");
                    if (context.Request.Query["X-Requested-With"] == "XMLHttpRequest" || context.Request.Headers["X-Requested-With"] == "XMLHttpRequest" ||
                        ex.HResult == (int)EventID.UploadFileError || ex.HResult == (int)EventID.UnsupportFile)
                    {
                        ProcessAjaxRequest(context, httpCode, ex.Message, redirectUrl, (EventID)ex.HResult);
                        return;
                    }
                    break;
            }

            if (!string.IsNullOrEmpty(redirectUrl))
            {
                context.Response.Clear();
                if (statusCode == HttpStatusCode.Unauthorized)
                {
                    //string username = context.GetBaseIdentity()?.AuditName;
                    //logger.Warn($"**** User:{username} Unauthorized, log out ****");

                    //await context.SignOutAsync(LEOAuthenticationConstants.LoginCookieName);
                    //await context.SignOutAsync(LEOAuthenticationConstants.PermissionCookieName);
                    //await context.SignOutAsync(LEOAuthenticationConstants.SingleSessionCookieName);
                }
                else if (statusCode != HttpStatusCode.MethodNotAllowed)
                {
                    string returnUrl = context.Request.AbsoluteUri() + "";
                    returnUrl = returnUrl.Replace("&", "%26");
                    if (redirectUrl.EndsWith("?"))
                    {
                        redirectUrl = string.Concat(redirectUrl, "ReturnUrl=", returnUrl);
                    }
                    else
                    {
                        redirectUrl = string.Concat(redirectUrl, "&ReturnUrl=", returnUrl);
                    }

                    var directForward = context.Request.Query["DirectForward"];
                    bool isDirectForward = false;
                    if (bool.TryParse(directForward, out isDirectForward) && isDirectForward)
                    {
                        redirectUrl = string.Concat("/Corebase/Login/Index?ReturnUrl=", returnUrl);
                    }
                }
                logger.Error($"Exception: will Redirect to {redirectUrl}");
                context.Response.Redirect(redirectUrl);
            }
        }

        private static async void ProcessAjaxRequest(HttpContext context, int httpCode, string errorMessage, string redirectUrl, EventID eventId = EventID.None)
        {
            //var statusCode = (HttpStatusCode)httpCode;
            //HttpContext.Current.Response.StatusCode = httpCode == 401 ? 403 : httpCode;
            //HttpContext.Current.Response.TrySkipIisCustomErrors = true;
            //if (statusCode == HttpStatusCode.MethodNotAllowed || statusCode == HttpStatusCode.Unused)
            //{
            //    HttpContext.Current.Response.ContentType = "text/html";
            //}
            //else
            //{
            //    errorMessage = Regex.Replace(errorMessage, @"^A public action method ('.*') was not found on controller ('.*')\.$", "The public action method was not found on the controller.");
            //    errorMessage = JsonConvert.SerializeObject(new { Error = errorMessage, Url = redirectUrl, EventID = eventId });
            //    HttpContext.Current.Response.ContentType = "application/json";
            //}
            //HttpContext.Current.Response.Write(errorMessage);
            //HttpContext.Current.Response.Flush();
            //HttpContext.Current.Response.End();


            var statusCode = (HttpStatusCode)httpCode;
            errorMessage = Regex.Replace(errorMessage, @"^A public action method ('.*') was not found on controller ('.*')\.$", "The public action method was not found on the controller.");
            errorMessage = JsonConvert.SerializeObject(new { error = errorMessage, url = redirectUrl, eventID = eventId });

            context.Response.Clear();
            context.Response.StatusCode = httpCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(errorMessage);
            await context.Response.Body.FlushAsync();
            await context.Response.CompleteAsync();

            logger.Warn($"Exception: will return json. Code:{httpCode}, Message:{errorMessage}, RedirectUrl:{redirectUrl},");
        }
    }
}
