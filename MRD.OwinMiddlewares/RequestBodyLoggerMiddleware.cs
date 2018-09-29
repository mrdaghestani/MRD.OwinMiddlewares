using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using Newtonsoft.Json;
using System.IO;
using System.Configuration;

namespace MRD.OwinMiddlewares
{
    public class RequestBodyLoggerMiddleware : OwinMiddleware
    {
        private ILogger _logger;
        private static readonly bool _isEnabled;

        static RequestBodyLoggerMiddleware()
        {
            _isEnabled = bool.Parse(ConfigurationManager.AppSettings["MRD.OwinMiddlewares.EnableRequestBodyLogger"] ?? "True");
        }
        public RequestBodyLoggerMiddleware(OwinMiddleware next, ILogger logger) : base(next)
        {
            _logger = logger;
        }
        public async override Task Invoke(IOwinContext context)
        {
            if (_isEnabled)
            {
                var excludeStarters = new[] { "/swagger", "/hangfire", "/ping" };

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
            }

            await Next.Invoke(context);
        }
    }
}