using Microsoft.AspNetCore.Http;
using NLog.LayoutRenderers;
using NLog.Web.LayoutRenderers;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utility
{
    [LayoutRenderer("elkmessage")]
    public class ELKLayoutRender : AspNetLayoutRendererBase
    {
        public ELKLayoutRender()
        {

        }

        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            //var currentUser = HttpContextAccessor?.HttpContext?.GetCurrentUser();
            var correlationId = HttpContextAccessor?.HttpContext?.TraceIdentifier;
            builder.Append(SerializerHelper.SerializeByJsonConvert(new
            {
                Level = logEvent.Level,
                Time = logEvent.TimeStamp.FormatDateTime("yyyy-MM-ddTHH:mm:ss.fffZ"),
                Thread = Thread.CurrentThread.ManagedThreadId,
                Logger = logEvent.LoggerName,
                Message = logEvent.FormattedMessage,
                ExceptionMessage = logEvent.Exception?.ToString(),
                //TenantId = currentUser?.tenantId,
                //currentUser = currentUser?.userName,
                CorrelationId = correlationId
            }));
        }

        protected override void DoAppend(StringBuilder builder, LogEventInfo logEvent)
        {
        }
    }
}
