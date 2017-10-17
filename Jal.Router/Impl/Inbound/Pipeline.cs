using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound
{
    public class Pipeline<TContent> : IPipeline
    {
        private readonly IComponentFactory _factory;

        private readonly Type[] _filters;  

        private int _current;

        private readonly IndboundMessageContext<TContent> _context;

        private readonly MiddlewareParameter _parameter;

        public Pipeline(IComponentFactory factory, Type[] filters, IndboundMessageContext<TContent> context, MiddlewareParameter parameter)
        {
            _factory = factory;
            _filters = filters;
            _current = 0;
            _parameter = parameter;
            _context = context;
        }

        public void Excute()
        {
            GetNext().Invoke();
        }

        private Action GetNext()
        {
            return () =>
            {
                if (_current < _filters.Length)
                {
                    var filter = _factory.Create<IMiddleware>(_filters[_current]);
                    _current++;
                    filter.Execute(_context, GetNext(), _parameter);
                }
            };
        }
    }
}