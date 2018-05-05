using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRD.OwinMiddlewares.Services
{
    public interface IApiKeyCheckService
    {
        Task<bool> Validate(string apiKey);
    }
}
