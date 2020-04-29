using System;
using System.Collections.Generic;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IOptionBuilder
    {
        void ClaimCheck();

        void CreateIfNotExist(IDictionary<string, object> properties = null, IList<Rule> rules = null);

        void Partition(Func<MessageContext, bool> condition = null);
    }
}