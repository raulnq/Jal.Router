using Jal.Router.Impl;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouterConfigurationSource
    {
        Route[] Source();
    }
}