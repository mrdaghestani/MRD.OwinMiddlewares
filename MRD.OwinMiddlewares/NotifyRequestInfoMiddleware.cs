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
        private Services.INotifyRequestService _notify;

        public NotifyRequestInfoMiddleware(OwinMiddleware next, Services.INotifyRequestService notify)
            : base(next)
        {
            _notify = notify;
        }
        public async override Task Invoke(IOwinContext context)
        {
            if (_notify != null)
            {
                await _notify.Notify(new DTOs.RequestInfo
                {
                    Id = Guid.NewGuid(),
                    Uri = context.Request.Uri,
                    Method = context.Request.Method,
                    QueryString = context.Request.QueryString,
                    LocalIpAddress = context.Request.LocalIpAddress,
                    RemoteIpAddress = context.Request.RemoteIpAddress,
                    XForwardedFor = context.Request.Headers.Where(x => x.Key.ToLower() == "X-Forwarded-For".ToLower()).FirstOrDefault().Value?.FirstOrDefault(),
                });
            }

            await Next.Invoke(context);
        }
    }
}