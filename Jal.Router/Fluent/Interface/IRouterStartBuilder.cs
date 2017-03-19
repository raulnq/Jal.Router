using Jal.Locator.Interface;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IRouterStartBuilder
    {
        IRouterBuilder UseServiceLocator(IServiceLocator serviceLocator);
    }
}