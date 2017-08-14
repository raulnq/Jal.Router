using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class ContinueRegisterRouteBuilder : IContinueRegisterRouteBuilder
    {
        private readonly Saga _saga;

        public ContinueRegisterRouteBuilder(Saga saga)
        {
            _saga = saga;
        }


        public INameRouteBuilder<THandler> RegisterRoute<THandler>(string name = "")
        {
            var builder = new NameRouteBuilder<THandler>(name, _saga.Routes);

            return builder;
        }
    }
}