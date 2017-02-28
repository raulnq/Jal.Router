using System;
using System.Collections.Generic;
using System.Linq;
using Jal.Factory.Interface;
using Jal.Factory.Model;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class MessageSenderProvider : IMessageSenderProvider
    {
        private readonly IObjectFactory _objectFactory;

        public MessageSenderProvider(IObjectFactory objectFactory)
        {
            _objectFactory = objectFactory;
        }

        public IMessageSender<TMessage>[] Provide<TMessage>(TMessage message, string route)
        {
            if (string.IsNullOrWhiteSpace(route))
            {
                return _objectFactory.Create<TMessage, IMessageSender<TMessage>>(message);
            }
            return _objectFactory.Create<TMessage, IMessageSender<TMessage>>(message, route);
        }

    }
}
