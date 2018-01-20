using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class NextRouteBuilder<TData> : INextRouteBuilder<TData>
    {
        private readonly Saga _saga;

        public NextRouteBuilder(Saga saga)
        {
            _saga = saga;
        }


        public INextListenerRouteBuilder<THandler, TData> RegisterHandler<THandler>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var builder = new NextNameRouteBuilder<THandler, TData>(_saga, name);

            return builder;
        }
    }
}