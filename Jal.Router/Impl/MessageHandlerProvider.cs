using System;
using System.Collections.Generic;
using System.Linq;
using Jal.Factory.Interface;
using Jal.Factory.Model;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class MessageHandlerProvider : IMessageHandlerProvider
    {
        private readonly IObjectFactory _objectFactory;

        public MessageHandlerProvider(IObjectFactory objectFactory)
        {
            _objectFactory = objectFactory;
        }

        public IMessageHandler<TMessage>[] Provide<TMessage>(TMessage message, string route)
        {
            if (string.IsNullOrWhiteSpace(route))
            {
                return _objectFactory.Create<TMessage, IMessageHandler<TMessage>>(message);
            }
            return _objectFactory.Create<TMessage, IMessageHandler<TMessage>>(message, route);
        }

    }
}
