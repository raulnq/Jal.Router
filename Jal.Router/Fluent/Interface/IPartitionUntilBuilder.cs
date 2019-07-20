using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IPartitionUntilBuilder
    {
        void Until(Func<MessageContext, bool> condition);
    }
}