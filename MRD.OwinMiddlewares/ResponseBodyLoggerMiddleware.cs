using Microsoft.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MRD.OwinMiddlewares
{
    public class ResponseBodyLoggerMiddleware : OwinMiddleware
    {
        private ILogger _logger;

        public ResponseBodyLoggerMiddleware(ILogger logger, OwinMiddleware next) : base(next)
        {
            _logger = logger;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var excludeStarters = new[] { "/swagger", "/hangfire" };

            if (!excludeStarters.Any(x => context.Request.Path.ToString().ToLower().StartsWith(x)) && _logger.IsDebugEnabled)
            {
                var responseHeaderText = JsonConvert.SerializeObject(context.Response.Headers);
                var responseStatusCode = context.Response.StatusCode;

                var bodyStream = context.Response.Body;

                var responseBodyStream = new MemoryStream();
                context.Response.Body = responseBodyStream;

                await Next.Invoke(context);

                responseBodyStream.Seek(0, SeekOrigin.Begin);
                var responseBodyText = new StreamReader(responseBodyStream).ReadToEnd();

                _logger.Debug($"RESPONSE HEADER: {responseHeaderText}{Environment.NewLine} STATUS CODE: {responseStatusCode}{Environment.NewLine} RESPONSE BODY: {responseBodyText}");

                responseBodyStream.Seek(0, SeekOrigin.Begin);
                await responseBodyStream.CopyToAsync(bodyStream);
                context.Response.Body = bodyStream;
            }
            else
            {
                await Next.Invoke(context);
            }
        }
    }
}