using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class NextRouteBuilder<TData> : INextRouteBuilder<TData>
    {
        private readonly Saga<TData> _saga;

        public NextRouteBuilder(Saga<TData> saga)
        {
            _saga = saga;
        }


        public INextNameRouteBuilder<THandler, TData> RegisterRoute<THandler>(string name = "")
        {
            var builder = new NextNameRouteBuilder<THandler, TData>(_saga, name);

            return builder;
        }
    }
}