using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using System.Configuration;

namespace MRD.OwinMiddlewares
{
    public class ApiKeyCheckMiddleware : Microsoft.Owin.OwinMiddleware
    {
        private Services.IApiKeyCheckService _apiKeyCheckService;
        private static readonly bool _isApiKeyCheckEnabled;

        static ApiKeyCheckMiddleware()
        {
            _isApiKeyCheckEnabled = bool.Parse(ConfigurationManager.AppSettings["MRD.OwinMiddlewares.EnableApiKeyCheck"] ?? "True");
        }

        public ApiKeyCheckMiddleware(OwinMiddleware next, Services.IApiKeyCheckService apiKeyCheckService)
            : base(next)
        {
            _apiKeyCheckService = apiKeyCheckService;
        }
        public async override Task Invoke(IOwinContext context)
        {
            if (_isApiKeyCheckEnabled)
            {
                //excluding 'swagger' and 'hangfire' from api-key check
                var excludeStarters = new[] { "/swagger", "/hangfire" };

                if (context.Request.Path.ToString() == "/" || excludeStarters.Any(x => context.Request.Path.ToString().ToLower().StartsWith(x)))
                {
                    await Next.Invoke(context);
                    return;
                }


                string apikey = context.Request.Headers.FirstOrDefault(h => h.Key == "apiKey").Value?.FirstOrDefault();

                if (!string.IsNullOrEmpty(apikey) && await _apiKeyCheckService.Validate(apikey))
                {
                    await Next.Invoke(context);
                    return;
                }

                context.Response.StatusCode = 401;
            }
            else
            {
                await Next.Invoke(context);
            }
        }
    }
}