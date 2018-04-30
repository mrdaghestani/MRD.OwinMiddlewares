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
        private Action<DTOs.RequestInfo> _notify;

        public NotifyRequestInfoMiddleware(OwinMiddleware next, Action<DTOs.RequestInfo> notify)
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

            return Next.Invoke(context);
        }
    }
}