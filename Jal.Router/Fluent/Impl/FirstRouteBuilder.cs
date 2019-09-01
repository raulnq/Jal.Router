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


        public IFirstListenerRouteBuilder<TData> RegisterHandler(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var builder = new FirstNameRouteBuilder<TData>(_saga, name);

            return builder;
        }
    }
}