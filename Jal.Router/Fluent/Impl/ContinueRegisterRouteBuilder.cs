using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class ContinueRegisterRouteBuilder<TData> : IContinueRegisterRouteBuilder<TData>
    {
        private readonly Saga<TData> _saga;

        public ContinueRegisterRouteBuilder(Saga<TData> saga)
        {
            _saga = saga;
        }


        public IContinueNameRouteBuilder<THandler, TData> RegisterRoute<THandler>(string name = "")
        {
            var builder = new ContinueNameRouteBuilder<THandler, TData>(_saga, name);

            return builder;
        }
    }
}