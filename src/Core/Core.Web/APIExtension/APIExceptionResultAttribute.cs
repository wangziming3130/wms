//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc;
//using System.Text;

//namespace Core.Web
//{
//    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
//    public class APIResultExceptionAttribute : ExceptionFilterAttribute
//    {
//        protected static readonly Siasunlogger  logger = Siasunlogger.GetInstance(MethodBase.GetCurrentMethod().DeclaringType);
//        public override async void OnException(ExceptionContext context)
//        {
//            if (context.Exception is CustomExceptionBase)
//            {
//                logger.Warn($"Hanlded exception has been thrown in action {context.ActionDescriptor?.DisplayName}, msg:{context.Exception}.");
//            }
//            else
//            {
//                logger.Error($"An error occured while executing action {context.ActionDescriptor?.DisplayName}.", context.Exception);
//            }

//            if (context.Exception is UnauthorizedAccessException)
//            {
//                context.Result = new ObjectResult(
//                   new
//                   {
//                       code = StatusCodes.Status401Unauthorized,
//                       status = ErrorStatus.Unauthorized
//                   });
//            }
//            else if (context.Exception is ErrorHandleException
//                || context.Exception is NotExistException)
//            {
//                context.Result = new ObjectResult(
//                   new
//                   {
//                       code = StatusCodes.Status200OK,
//                       status = ErrorStatus.ModelInfoInvalid,
//                       message = context.Exception.Message
//                   });
//            }
//            else if (context.Exception is AccessDeniedException || context.Exception is HttpUnhandledException)
//            {
//                //context.Result = new ObjectResult(
//                //    new
//                //    {
//                //        code = StatusCodes.Status403Forbidden,
//                //        status = ErrorStatus.AccessDenied,
//                //    });
//                ExceptionMessage subCode = ExceptionMessage.CF_NoAuthorize_NoPermission_Message;

//                if (context.Exception.Message.Equals(ExceptionMessage.CF_NoAuthorize_Simulation_Message.ToDescription()))
//                {
//                    subCode = ExceptionMessage.CF_NoAuthorize_Simulation_Message;
//                }
//                else if (context.Exception.Message.Equals(ExceptionMessage.CF_NoAuthorize_DraftModule_Message.ToDescription()))
//                {
//                    subCode = ExceptionMessage.CF_NoAuthorize_DraftModule_Message;
//                }

//                context.Result = new ObjectResult(
//                    new
//                    {
//                        code = StatusCodes.Status405MethodNotAllowed,//StatusCodes.Status403Forbidden,
//                        subCode = (int)subCode,
//                        status = ErrorStatus.AccessDenied,
//                    });
//            }
//            else if (context.Exception is ResourceNotFoundException)
//            {
//                context.Result = new ObjectResult(
//                    new
//                    {
//                        code = StatusCodes.Status404NotFound,
//                        status = ErrorStatus.AccessDenied,
//                    });
//            }
//            else if (context.Exception is UnExpectedException)
//            {
//                context.Result = new ObjectResult(
//                    new
//                    {
//                        code = StatusCodes.Status501NotImplemented,
//                        status = ErrorStatus.AccessDenied,
//                    });
//            }
//            else if (context.Exception is Error500Exception)
//            {
//                context.Result = new ObjectResult(
//                   new
//                   {
//                       code = StatusCodes.Status500InternalServerError,
//                       status = ErrorStatus.Error500Page
//                   });
//            }
//            else if (context.Exception is Error404Exception)
//            {
//                context.Result = new ObjectResult(
//                   new
//                   {
//                       code = StatusCodes.Status404NotFound
//                   });
//            }
//            else if (context.Exception is WarningHandleException)
//            {
//                context.Result = new ObjectResult(
//                   new
//                   {
//                       code = StatusCodes.Status200OK,
//                       status = ErrorStatus.WarningInfo,
//                       message = context.Exception.Message
//                   });
//            }
//            else if (context.Exception is WrongVideoStatusException)
//            {
//                context.Result = new ObjectResult(
//                   new
//                   {
//                       code = StatusCodes.Status200OK,
//                       status = StatusCodes.Status200OK,
//                       message = context.Exception.Message
//                   });
//            }
//            else
//            {
//                context.Result = new ObjectResult(
//                   new
//                   {
//                       code = StatusCodes.Status200OK,
//                       status = ErrorStatus.RequestError,
//                       //message = context.Exception.Message
//                   });
//            }

//            string url = context.HttpContext.Request.AbsoluteUri();
//            if (string.IsNullOrEmpty(url))
//            {
//                string action = context.RouteData.Values["action"].ToString();
//                string controller = context.RouteData.Values["controller"].ToString();
//                string queryParam = context.HttpContext.Request.QueryString.ToString();
//                url = $"{controller}/{action}?{queryParam}";
//            }


//            context.HttpContext.Request.Body.Position = 0;
//            string bodyStr = string.Empty;
//            using (var reader = new StreamReader(context.HttpContext.Request.Body, Encoding.UTF8, true, 1024, true))
//            {
//                var bodyRead = reader.ReadToEndAsync();
//                bodyStr = bodyRead.Result;  //把body赋值给bodyStr
//            }

//            //log
//            logger.Error(String.Format("request url:{0},body:{1},Error:{2}", url, bodyStr, context.Exception));
//        }
//    }
//    public enum ErrorStatus
//    {
//        RequestError = 1000,
//        ModelInfoInvalid = 1200,
//        Unauthorized = 1400,
//        Error500Page = 1500,
//        AccessDenied = 1600,
//        WarningInfo = 1700,
//        LicenseExpired = 1800
//    }
//}
