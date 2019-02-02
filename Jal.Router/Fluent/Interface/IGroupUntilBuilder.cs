using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IGroupUntilBuilder
    {
        void Until(Func<MessageContext, bool> condition);
    }
}