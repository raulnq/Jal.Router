using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IOptionBuilder
    {
        void ClaimCheck();

        void Partition(Func<MessageContext, bool> condition = null);
    }
}