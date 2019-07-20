using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Handler
    {
        public Type Type { get; }
        public IDictionary<string, object> Parameters { get; }

        public Handler(Type type, IDictionary<string, object> parameter)
        {
            Type = type;
            Parameters = parameter;
        }
    }
}