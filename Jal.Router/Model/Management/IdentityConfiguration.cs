using System;

namespace Jal.Router.Model.Management
{
    public class IdentityConfiguration
    {
        public Func<MessageContext, string> OperationIdBuilder { get; set; }
        public Func<MessageContext, string> ParentIdBuilder { get; set; }
        public Func<MessageContext, string> IdBuilder { get; set; }

        public IdentityConfiguration()
        {
            OperationIdBuilder = null;
            ParentIdBuilder = null;
            IdBuilder = null;
        }
    }
}