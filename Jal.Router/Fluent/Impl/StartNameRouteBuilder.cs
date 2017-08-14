using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class StartNameRouteBuilder<THandler, TData> : IStartNameRouteBuilder<THandler, TData>
    {
        private readonly string _name;

        public List<Route> Routes { get; set; }
        private readonly Saga _saga;

        public StartNameRouteBuilder(Saga saga, string name)
        {
            _saga = saga;
            _name = name;
        }

        public IHandlerBuilder<TContent, THandler> ForMessage<TContent>(Func<TData, string> key)
        {
            var value = new Route<TContent, THandler>(_name) { First = true };

            var builder = new HandlerBuilder<TContent, THandler>(value);

            //Remove existing First
            _saga.Routes.Add(value);

            return builder;
        }
    }
}