using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roza.OwinMiddlewares
{
    public interface ILogger
    {
        bool IsDebugEnabled { get; }
        bool IsFatalEnabled { get; }
        void Debug(string log);
        void Fatal(Exception exception);
    }
}
