using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace MRD.OwinMiddlewares.DTOs
{
    public class RequestInfo
    {
        public Guid Id { get; set; }
        public Uri Uri { get; set; }
        public string Method { get; set; }
        public QueryString QueryString { get; set; }
        public string LocalIpAddress { get; set; }
        public string RemoteIpAddress { get; set; }
        public string XForwardedFor { get; set; }
    }
}
