using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using Newtonsoft.Json;
using System.IO;

namespace Roza.OwinMiddlewares
{
    public class RequestBodyLoggerMiddleware : OwinMiddleware
    {
        private ILogger _logger;

        public RequestBodyLoggerMiddleware(ILogger logger, OwinMiddleware next) : base(next)
        {
            _logger = logger;
        }
        public async override Task Invoke(IOwinContext context)
        {
            var excludeStarters = new[] { "/swagger", "/hangfire" };

            if (!excludeStarters.Any(x => context.Request.Path.ToString().ToLower().StartsWith(x)) && _logger.IsDebugEnabled)
            {
                var requestHeaderText = JsonConvert.SerializeObject(context.Request.Headers);

                var requestBodyStream = new MemoryStream();
                await context.Request.Body.CopyToAsync(requestBodyStream);

                requestBodyStream.Seek(0, SeekOrigin.Begin);
                var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();
                _logger.Debug($"REQUEST HEADER: {requestHeaderText}{Environment.NewLine} REQUEST BODY: {requestBodyText}");

                requestBodyStream.Seek(0, SeekOrigin.Begin);
                context.Request.Body = requestBodyStream;
            }

            await Next.Invoke(context);
        }
    }
}