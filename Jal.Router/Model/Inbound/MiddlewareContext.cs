using System;

namespace Jal.Router.Model.Inbound
{
    public class MiddlewareContext
    {
        public int Index { get; set; }
        public Type[] MiddlewareTypes { get; set; }
    }
}