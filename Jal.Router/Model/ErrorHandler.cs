using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{

    public class ErrorHandler : Handler
    {
        public IList<Type> ExceptionTypes { get; }

        public bool StopAfterHandle { get; }

        public ErrorHandler(Type type, IDictionary<string, object> parameter, bool stopafterhandle): base(type, parameter)
        {
            ExceptionTypes = new List<Type>();
            StopAfterHandle = stopafterhandle;
        }
    }
}