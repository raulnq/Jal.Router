using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class StartingRouteBuilder<TData> : IStartingRouteBuilder<TData>
    {
        private readonly Saga _saga;

        public StartingRouteBuilder(Saga saga)
        {
            _saga = saga;
        }


        public IStartingListenerRouteBuilder<THandler, TData> RegisterRoute<THandler>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var builder = new StartingNameRouteBuilder<THandler, TData>(_saga, name);

            return builder;
        }
    }
}