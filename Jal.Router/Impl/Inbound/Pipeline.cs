using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound
{
    public class Pipeline<TContent> : IPipeline
    {
        private readonly IComponentFactory _factory;

        private readonly Type[] _middlewares;  

        private int _current;

        private readonly IndboundMessageContext<TContent> _context;

        private readonly MiddlewareParameter _parameter;

        public Pipeline(IComponentFactory factory, Type[] middlewares, IndboundMessageContext<TContent> context, MiddlewareParameter parameter)
        {
            _factory = factory;
            _middlewares = middlewares;
            _current = 0;
            _parameter = parameter;
            _context = context;
        }

        public void Execute()
        {
            GetNext().Invoke();
        }

        private Action GetNext()
        {
            return () =>
            {
                if (_current < _middlewares.Length)
                {
                    var middleware = _factory.Create<IMiddleware>(_middlewares[_current]);
                    _current++;
                    middleware.Execute(_context, GetNext(), _parameter);
                }
            };
        }
    }
}