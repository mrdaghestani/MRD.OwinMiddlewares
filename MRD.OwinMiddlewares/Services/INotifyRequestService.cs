using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRD.OwinMiddlewares.Services
{
    public interface INotifyRequestService
    {
        void Notify(DTOs.RequestInfo requestInfo);
    }
}
