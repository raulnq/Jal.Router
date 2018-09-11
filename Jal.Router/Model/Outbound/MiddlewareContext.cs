using System;

namespace Jal.Router.Model.Outbound
{
    public class MiddlewareContext
    {
        public Options Options { get; set; }

        public string OutboundType { get; set; }

        public Channel Channel { get; set; }

        public Type ResultType { get; set; }

        public int Index { get; set; }

        public Type[] MiddlewareTypes { get; set; }
    }
}