using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MRD.OwinMiddlewares
{
    public class ExceptionLoggerMiddleware : OwinMiddleware
    {
        private ILogger _logger;

        public ExceptionLoggerMiddleware(OwinMiddleware next, ILogger logger)
            : base(next)
        {
            _logger = logger;
        }

        public override async Task Invoke(IOwinContext context)
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

    }
}