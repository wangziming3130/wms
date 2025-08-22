using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;

    public static class HttpRequestExtensions
    {
        public static IActionResult OK<T>(this HttpRequest request, T value)
        {
            return new OkObjectResult(value);
        }

        public static IActionResult Created<T>(this HttpRequest request, Uri uri, T value)
        {
            return new CreatedResult(uri, value);
        }

        public static IActionResult Created<T>(this HttpRequest request, String location, T value)
        {
            return new CreatedResult(location, value);
        }

        public static IActionResult CreatedAtRoute<T>(this HttpRequest request, string routeName, object routeValues, T value)
        {
            if (routeName == null)
            {
                throw new ArgumentNullException(nameof(routeName));
            }
            return new CreatedAtRouteResult(routeName, routeValues, value);
        }

        public static IActionResult Json<T>(this HttpRequest request, T value, HttpStatusCode status)
        {
            return new JsonResult(value) { StatusCode = (int)status };
        }

        public static IActionResult Accepted<T>(this HttpRequest request, T value)
        {
            return new AcceptedResult(location: null, value: value);
        }

        public static IActionResult NoContent(this HttpRequest request)
        {
            return new NoContentResult();
        }

        // public static IActionResult BadRequest(this HttpRequest request, params ApiError[] errors)
        // {
        //     if (errors != null && errors.Any())
        //         return new BadRequestObjectResult(errors);
        //     else
        //         return new BadRequestResult();
        // }
        //
        // public static IActionResult NotFound(this HttpRequest request, params ApiError[] errors)
        // {
        //     if (errors != null && errors.Any())
        //         return new NotFoundObjectResult(errors);
        //     else
        //         return new NotFoundResult();
        // }

        public static IActionResult FileStream(this HttpRequest request, Stream fileStream, string contentType, string fileDownLoadName)
        {
            return new FileStreamResult(fileStream, contentType)
            {
                FileDownloadName = fileDownLoadName
            };
        }

        public static IActionResult FileContent(this HttpRequest request, byte[] fileContents, string contentType, string fileDownLoadName)
        {
            return new FileContentResult(fileContents, contentType)
            {
                FileDownloadName = fileDownLoadName
            };
        }

        public static string AbsoluteUri(this HttpRequest request)
        {
            return string.Concat(
                        request.Scheme,
                        "://",
                        request.Host.ToUriComponent(),
                        request.PathBase.ToUriComponent(),
                        request.Path.ToUriComponent(),
                        request.QueryString.ToUriComponent());
        }
        /// <summary>
        /// Determines whether the specified HTTP request is an AJAX request.
        /// </summary>
        /// 
        /// <returns>
        /// true if the specified HTTP request is an AJAX request; otherwise, false.
        /// </returns>
        /// <param name="request">The HTTP request.</param><exception cref="T:System.ArgumentNullException">The <paramref name="request"/> parameter is null (Nothing in Visual Basic).</exception>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
    }
}
