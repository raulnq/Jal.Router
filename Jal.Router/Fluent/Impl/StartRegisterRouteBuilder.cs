using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class StartRegisterRouteBuilder<TData> : IStartRegisterRouteBuilder<TData>
    {
        private readonly Saga _saga;

        public StartRegisterRouteBuilder(Saga saga)
        {
            _saga = saga;
        }


        public IStartNameRouteBuilder<THandler, TData> RegisterRoute<THandler>(string name = "")
        {
            var builder = new StartNameRouteBuilder<THandler, TData>(_saga, name);

            return builder;
        }
    }
}