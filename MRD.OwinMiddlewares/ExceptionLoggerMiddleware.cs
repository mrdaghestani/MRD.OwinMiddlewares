using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MRD.OwinMiddlewares
{
    public class ExceptionLoggerMiddleware : OwinMiddleware
    {
        private ILogger _logger;
        private static readonly bool _isEnabled;

        static ExceptionLoggerMiddleware()
        {
            _isEnabled = bool.Parse(ConfigurationManager.AppSettings["MRD.OwinMiddlewares.EnableExceptionLogger"] ?? "True");
        }
        public ExceptionLoggerMiddleware(OwinMiddleware next, ILogger logger)
            : base(next)
        {
            _logger = logger;
        }

        public override async Task Invoke(IOwinContext context)
        {
            if (_isEnabled)
            {
                try
                {
                    await Next.Invoke(context);
                }
                catch (Exception ex)
                {
                    if (_logger.IsFatalEnabled)
                    {
                        _logger.Fatal(ex);
                    }
                    throw;
                }
            }
            else
            {
                await Next.Invoke(context);
            }
        }

    }
}