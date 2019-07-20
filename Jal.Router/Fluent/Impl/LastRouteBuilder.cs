using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class LastRouteBuilder<TData> : ILastRouteBuilder<TData>
    {
        private readonly Saga _saga;

        public LastRouteBuilder(Saga saga)
        {
            _saga = saga;
        }


        public ILastListenerRouteBuilder<THandler, TData> RegisterHandler<THandler>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var builder = new LastNameRouteBuilder<THandler, TData>(_saga, name);

            return builder;
        }

        public ILastListenerRouteBuilder<THandler, TData> RegisterHandler<THandler>()
        {
            return RegisterHandler<THandler>(typeof(THandler).Name.ToLower());
        }
    }
}