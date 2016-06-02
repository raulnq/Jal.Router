using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IRequestRouterEndFluentBuilder
    {
        IRequestRouter Create { get; }
    }
}