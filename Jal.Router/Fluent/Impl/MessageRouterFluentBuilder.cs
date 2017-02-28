using System;
using Jal.Factory.Interface;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Impl
{
    public class MessageRouterFluentBuilder : IMessageRouterFluentBuilder, IMessageRouterStartFluentBuilder
    {
        private IMessageHandlerFactory _messageHandlerFactory;

        private IMessagetRouterInterceptor _messagetRouterInterceptor;

        public IMessageRouterFluentBuilder UseObjectFactory(IObjectFactory objectFactory)
        {
            if (objectFactory == null)
            {
                throw new ArgumentNullException(nameof(objectFactory));
            }
            _messageHandlerFactory = new MessageHandlerFactory(objectFactory);
            return this;
        }


        public IMessageRouter Create
        {
            get
            {
                var result = new MessageRouter(_messageHandlerFactory);

                if (_messagetRouterInterceptor != null)
                {
                    result.Interceptor = _messagetRouterInterceptor;
                }

                return result;
            }
            
        }

        public IMessageRouterFluentBuilder UseInterceptor(IMessagetRouterInterceptor messagetRouterInterceptor)
        {
            if (messagetRouterInterceptor == null)
            {
                throw new ArgumentNullException(nameof(messagetRouterInterceptor));
            }
            _messagetRouterInterceptor = messagetRouterInterceptor;
            return this;
        }
    }
}