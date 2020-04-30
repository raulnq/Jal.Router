using Jal.Router.Model;
using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IWhenEndpointBuilder : IOnEndPointOptionBuilder
    {
        IOnEndPointOptionBuilder When(Func<Options, object, bool> condition);
    }
}