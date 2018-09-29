using Microsoft.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MRD.OwinMiddlewares
{
    public class ResponseBodyLoggerMiddleware : OwinMiddleware
    {
        private ILogger _logger;
        private static readonly bool _isEnabled;

        static ResponseBodyLoggerMiddleware()
        {
            _isEnabled = bool.Parse(ConfigurationManager.AppSettings["MRD.OwinMiddlewares.EnableResponseBodyLogger"] ?? "True");
        }
        public ResponseBodyLoggerMiddleware(OwinMiddleware next, ILogger logger) : base(next)
        {
            _logger = logger;
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (_isEnabled)
            {
                var excludeStarters = new[] { "/swagger", "/hangfire", "/ping" };

                if (!excludeStarters.Any(x => context.Request.Path.ToString().ToLower().StartsWith(x)) && _logger.IsDebugEnabled)
                {
                    var responseHeaderText = JsonConvert.SerializeObject(context.Response.Headers);
                    var responseStatusCode = context.Response.StatusCode;

                    var bodyStream = context.Response.Body;

                    var responseBodyStream = new MemoryStream();
                    context.Response.Body = responseBodyStream;

                    try
                    {
                        await Next.Invoke(context);
                    }
                    catch
                    {
                        context.Response.Body = bodyStream;
                        throw;
                    }

                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    var responseBodyText = new StreamReader(responseBodyStream).ReadToEnd();

                    _logger.Debug($"RESPONSE HEADER: {responseHeaderText}{Environment.NewLine} STATUS CODE: {responseStatusCode}{Environment.NewLine} RESPONSE BODY: {responseBodyText}");

                    responseBodyStream.Seek(0, SeekOrigin.Begin);
                    await responseBodyStream.CopyToAsync(bodyStream);
                    context.Response.Body = bodyStream;

                    return;
                }
            }

            await Next.Invoke(context);
        }
    }
}