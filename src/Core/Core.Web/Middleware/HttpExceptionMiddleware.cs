using Core.Domain;
using Core.Utility;
using Microsoft.AspNetCore.Http.Features;

namespace Core.Web
{
    public class HttpExceptionMiddleware
    {
        private static SiasunLogger logger = SiasunLogger.GetInstance(typeof(HttpExceptionMiddleware));

        private readonly RequestDelegate _next;

        public HttpExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (HttpException httpException)
            {
                logger.Error($"Http Exception: Code:{httpException.StatusCode}, Message:{httpException.Message}");
                context.Response.StatusCode = httpException.StatusCode;
                var responseFeature = context.Features.Get<IHttpResponseFeature>();
                responseFeature.ReasonPhrase = httpException.Message;
            }
        }
    }
}
