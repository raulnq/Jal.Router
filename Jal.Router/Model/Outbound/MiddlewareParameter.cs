using System;

namespace Jal.Router.Model.Outbound
{
    public class MiddlewareParameter
    {
        public Options Options { get; set; }

        public string OutboundType { get; set; }

        public Channel Channel { get; set; }

        public object Result { get; set; }

        public Type ResultType { get; set; }
    }
}