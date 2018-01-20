using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class EndingRouteBuilder<TData> : IEndingRouteBuilder<TData>
    {
        private readonly Saga _saga;

        public EndingRouteBuilder(Saga saga)
        {
            _saga = saga;
        }


        public IEndingListenerRouteBuilder<THandler, TData> RegisterHandler<THandler>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var builder = new EndingNameRouteBuilder<THandler, TData>(_saga, name);

            return builder;
        }
    }
}