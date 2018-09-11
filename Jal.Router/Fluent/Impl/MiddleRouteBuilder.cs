using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class MiddleRouteBuilder<TData> : IMiddleRouteBuilder<TData>
    {
        private readonly Saga _saga;

        public MiddleRouteBuilder(Saga saga)
        {
            _saga = saga;
        }


        public IMiddleListenerRouteBuilder<THandler, TData> RegisterHandler<THandler>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var builder = new MiddleNameRouteBuilder<THandler, TData>(_saga, name);

            return builder;
        }
    }
}