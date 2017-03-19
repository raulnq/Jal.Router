using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Jal.Router.Fluent.Impl;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{

    public abstract class AbstractRouterConfigurationSource : IRouterConfigurationSource
    {
        private readonly List<Route> _routes = new List<Route>();

        public Route[] Source()
        {
            return _routes.ToArray();
        }

        public INameRouteBuilder<TConsumer> RegisterRoute<TConsumer>(string name="")
        {
            var builder = new NameRouteBuilder<TConsumer>(name, _routes);

            return builder;
        }
    }
}
