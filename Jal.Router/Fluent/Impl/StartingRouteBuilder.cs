using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class StartingRouteBuilder<TData> : IStartingRouteBuilder<TData>
    {
        private readonly Saga<TData> _saga;

        public StartingRouteBuilder(Saga<TData> saga)
        {
            _saga = saga;
        }


        public IStartingNameRouteBuilder<THandler, TData> RegisterRoute<THandler>(string name = "")
        {
            var builder = new StartingNameRouteBuilder<THandler, TData>(_saga, name);

            return builder;
        }
    }
}