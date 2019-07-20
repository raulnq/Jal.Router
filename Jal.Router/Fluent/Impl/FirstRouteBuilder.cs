using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class FirstRouteBuilder<TData> : IFirstRouteBuilder<TData>
    {
        private readonly Saga _saga;

        public FirstRouteBuilder(Saga saga)
        {
            _saga = saga;
        }


        public IFirstListenerRouteBuilder<THandler, TData> RegisterHandler<THandler>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var builder = new FirstNameRouteBuilder<THandler, TData>(_saga, name);

            return builder;
        }

        public IFirstListenerRouteBuilder<THandler, TData> RegisterHandler<THandler>()
        {
            return RegisterHandler<THandler>(typeof(THandler).Name.ToLower());
        }
    }
}