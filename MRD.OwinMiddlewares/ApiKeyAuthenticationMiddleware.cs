using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using System.Configuration;

namespace Roza.OwinMiddlewares
{
    public class ApiKeyCheckMiddleware : Microsoft.Owin.OwinMiddleware
    {
        private string _apiKey;
        public ApiKeyCheckMiddleware(OwinMiddleware next, string apiKeySettingsName)
            : base(next)
        {
            _apiKey = ConfigurationManager.AppSettings[apiKeySettingsName ?? "ApiKey"];
        }
        public async override Task Invoke(IOwinContext context)
        {
            //excluding 'swagger' and 'hangfire' from api-key check
            var excludeStarters = new[] { "/swagger", "/hangfire" };

            if (context.Request.Path.ToString() == "/" ||
                excludeStarters.Any(x => context.Request.Path.ToString().ToLower().StartsWith(x)))
            {
                await Next.Invoke(context);
                return;
            }


            string apikey = context.Request.Headers.FirstOrDefault(h => h.Key == "apiKey").Value?.FirstOrDefault();

            if (!string.IsNullOrEmpty(apikey) && (apikey == _apiKey))
            {
                await Next.Invoke(context);
                return;
            }

            context.Response.StatusCode = 401;
        }
    }
}