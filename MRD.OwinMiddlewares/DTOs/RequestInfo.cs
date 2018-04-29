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
        public Guid Id { get; internal set; }
        public Uri Uri { get; internal set; }
        public string Method { get; internal set; }
        public QueryString QueryString { get; internal set; }
        public string LocalIpAddress { get; internal set; }
        public string RemoteIpAddress { get; internal set; }
        public string XForwardedFor { get; internal set; }
    }
}
