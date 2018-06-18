using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl.Outbound
{
    public class Pipeline : IPipeline
    {
        private readonly IComponentFactory _factory;

        private readonly Type[] _middlewares;  

        private int _current;

        private readonly MessageContext _context;

        private readonly MiddlewareParameter _parameter;

        public Pipeline(IComponentFactory factory, Type[] middlewares, MessageContext context, MiddlewareParameter parameter)
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
                    middleware.Execute(_context, GetNext(), GetCurrent(), _parameter);
                }
            };
        }

        private Action GetCurrent()
        {
            return () =>
            {
                var index = _current - 1;
                if (index < _middlewares.Length)
                {
                    var middleware = _factory.Create<IMiddleware>(_middlewares[index]);
                    middleware.Execute(_context, GetNext(), GetCurrent(), _parameter);
                }
            };
        }
    }
}