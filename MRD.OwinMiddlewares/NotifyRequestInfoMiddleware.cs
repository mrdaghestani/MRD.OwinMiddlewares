using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;

namespace MRD.OwinMiddlewares
{
    public class NotifyRequestInfoMiddleware : Microsoft.Owin.OwinMiddleware
    {
        internal const string RequestIdKey = "RequestId";
        internal const string MethodKey = "Method";
        internal const string URLKey = "URL";
        internal const string QueryStringKey = "QueryString";
        internal const string LocalIpAddressKey = "LocalIp";
        internal const string RemoteIpAddressKey = "RemoteIp";
        internal const string XForwardedForKey = "XForwardedFor";
        private Action<DTOs.RequestInfo> _notify;

        public NotifyRequestInfoMiddleware(Action<DTOs.RequestInfo> notify, OwinMiddleware next)
            : base(next)
        {
            _notify = notify;
        }
        public override Task Invoke(IOwinContext context)
        {
            _notify?.Invoke(new DTOs.RequestInfo
            {
                Id = Guid.NewGuid(),
                Uri = context.Request.Uri,
                Method = context.Request.Method,
                QueryString = context.Request.QueryString,
                LocalIpAddress = context.Request.LocalIpAddress,
                RemoteIpAddress = context.Request.RemoteIpAddress,
                XForwardedFor = context.Request.Headers.Where(x => x.Key.ToLower() == "X-Forwarded-For".ToLower()).FirstOrDefault().Value?.FirstOrDefault(),
            });

            //NLog.MappedDiagnosticsLogicalContext.Set(RequestIdKey, Guid.NewGuid());
            //NLog.MappedDiagnosticsLogicalContext.Set(URLKey, context.Request.Uri?.ToString());
            //NLog.MappedDiagnosticsLogicalContext.Set(MethodKey, context.Request.Method);
            //NLog.MappedDiagnosticsLogicalContext.Set(QueryStringKey, context.Request.QueryString.HasValue ? context.Request.QueryString.Value.ToString() : string.Empty);
            //NLog.MappedDiagnosticsLogicalContext.Set(LocalIpAddressKey, context.Request.LocalIpAddress);
            //NLog.MappedDiagnosticsLogicalContext.Set(RemoteIpAddressKey, context.Request.RemoteIpAddress);
            //NLog.MappedDiagnosticsLogicalContext.Set(XForwardedForKey, context.Request.Headers.Where(x => x.Key.ToLower() == "X-Forwarded-For".ToLower()).FirstOrDefault().Value?.FirstOrDefault());


            return Next.Invoke(context);
        }
    }
}