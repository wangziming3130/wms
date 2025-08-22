using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Core.Web
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class APIObjectResultAttribute : ActionFilterAttribute
    {
        private static readonly string MSG_Success = "success"; //1xx,2xx,3xx

        //private static readonly string MSG_Fail = "fail"; //5xx

        private static readonly string MSG_Error = "error"; //4xx

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ObjectResult)
            {
                if (context.Result is OkObjectResult)
                {
                    context.Result = new ObjectResult(new { code = StatusCodes.Status200OK, status = MSG_Success, message = MSG_Success, data = (context.Result as ObjectResult).Value });
                }
                else if (context.Result is BadRequestObjectResult)
                {
                    context.Result = new ObjectResult(new { code = StatusCodes.Status400BadRequest, status = MSG_Error, message = "", data = (context.Result as BadRequestObjectResult).Value });
                }
                else if (context.Result is UnauthorizedObjectResult)
                {
                    context.Result = new ObjectResult(new { code = StatusCodes.Status401Unauthorized, status = MSG_Error, message = "" });
                }
                else if (context.Result is NotFoundObjectResult || context.Result is NotFoundResult)
                {
                    context.Result = new ObjectResult(new { code = StatusCodes.Status404NotFound, status = MSG_Error, message = "" });
                }
                else if (context.Result is AcceptedAtActionResult || context.Result is AcceptedAtRouteResult || context.Result is AcceptedResult)
                {
                    context.Result = new ObjectResult(new { code = StatusCodes.Status202Accepted, status = MSG_Success, message = "" });
                }
                else if (context.Result is CreatedAtActionResult || context.Result is CreatedAtRouteResult || context.Result is CreatedResult)
                {
                    context.Result = new ObjectResult(new { code = StatusCodes.Status201Created, status = MSG_Success, message = "" });
                }
                else if (context.Result is ConflictObjectResult)
                {
                    context.Result = new ObjectResult(new { code = StatusCodes.Status409Conflict, status = MSG_Error, message = "" });
                }
                else if (context.Result is UnprocessableEntityObjectResult)
                {
                    context.Result = new ObjectResult(new { code = StatusCodes.Status422UnprocessableEntity, status = MSG_Error, message = "" });
                }
                else
                {
                    var objR = context.Result as ObjectResult;
                    context.Result = new ObjectResult(new { code = objR.StatusCode.HasValue ? objR.StatusCode : StatusCodes.Status200OK, data = objR.Value });
                    //if (objR.Value == null)
                    //{
                    //    context.Result = new ObjectResult(new { code = StatusCodes.Status404NotFound, status = MSG_Error, message = "uncover result type." });

                    //}
                    //else
                    //{
                    //    context.Result = new ObjectResult(new { code = StatusCodes.Status200OK, status = MSG_Success, message = "uncover result type.", data = objR.Value });
                    //}
                }
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(new { code = StatusCodes.Status404NotFound, status = MSG_Error, message = "Resource Not Found." });
            }
            else if (context.Result is ContentResult)
            {
                context.Result = new ObjectResult(new { code = StatusCodes.Status200OK, status = MSG_Success, message = "", data = (context.Result as ContentResult).Content });
            }
            else if (context.Result is StatusCodeResult)
            {
                context.Result = new ObjectResult(new { code = (context.Result as StatusCodeResult).StatusCode, status = "", message = "" });
            }
            else if (context.Result is JsonResult)
            {
                //if (!(context.Controller is RestController))
                    context.Result = new JsonResult(new { code = StatusCodes.Status200OK, status = MSG_Success, message = MSG_Success, data = (context.Result as JsonResult).Value });
            }
            else if (context.Result is RedirectResult || context.Result is FileResult)
            {
                //Do nothing
            }
            else
            {
                var resultObj = context.Result as ObjectResult;
                context.Result = new ObjectResult(new { code = resultObj.StatusCode.HasValue ? resultObj.StatusCode : StatusCodes.Status200OK, data = resultObj.Value });
                //if (objR.Value == null)
                //{
                //    context.Result = new ObjectResult(new { code = StatusCodes.Status404NotFound, status = MSG_Error, message = "uncover result type." });

                //}
                //else
                //{
                //    context.Result = new ObjectResult(new { code = StatusCodes.Status200OK, status = MSG_Success, message = "uncover result type.", data = objR.Value });
                //}
            }
        }
    }
}
