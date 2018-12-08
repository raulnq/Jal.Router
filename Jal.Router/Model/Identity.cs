using System;

namespace Jal.Router.Model
{
    public class Identity
    {
        public Func<MessageContext, IdentityContext> Builder { get; set; }
        public Identity()
        {
            Builder = null;
        }
    }
}