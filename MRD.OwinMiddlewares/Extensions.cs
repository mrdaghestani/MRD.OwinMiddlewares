using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRD.OwinMiddlewares
{
    public static class Extensions
    {
        public static IAppBuilder NotifyRequestInfo(this IAppBuilder app, Action<DTOs.RequestInfo> notify)
        {
            return app.Use<NotifyRequestInfoMiddleware>(notify);
        }
        public static IAppBuilder UseRequestBodyLogger(this IAppBuilder app, ILogger logger)
        {
            return app.Use<RequestBodyLoggerMiddleware>(logger);
        }
        public static IAppBuilder UseResponseBodyLogger(this IAppBuilder app, ILogger logger)
        {
            return app.Use<ResponseBodyLoggerMiddleware>(logger);
        }
        public static IAppBuilder UseExceptionLogger(this IAppBuilder app, ILogger logger)
        {
            return app.Use<ExceptionLoggerMiddleware>();
        }
        public static IAppBuilder UseApiKeyCheck(this IAppBuilder app, string apiKeySettingsName = null)
        {
            return app.Use<ApiKeyCheckMiddleware>();
        }
    }
}