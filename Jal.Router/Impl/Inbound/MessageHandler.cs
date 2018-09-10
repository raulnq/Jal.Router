using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound
{
    public class MessageHandler : IMiddleware
    {
        private readonly IMessageRouter _router;

        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public MessageHandler(IMessageRouter router, IComponentFactory factory, IConfiguration configuration)
        {
            _router = router;
            _factory = factory;
            _configuration = configuration;
        }

        public void Execute(MessageContext messagecontext, Action<MessageContext, MiddlewareContext> next, MiddlewareContext middlewarecontext)
        {
            messagecontext.AddTrack(messagecontext.Identity, messagecontext.Origin, messagecontext.Route);

            if (_configuration.Storage.SaveMessage)
            {
                var storage = _factory.Create<ISagaStorage>(_configuration.SagaStorageType);

                var messageentity = new MessageEntity()
                {
                    Content = messagecontext.Content,
                    ContentType = messagecontext.Route.ContentType.FullName,
                    Identity = messagecontext.Identity,
                    Version = messagecontext.Version,
                    RetryCount = messagecontext.RetryCount,
                    LastRetry = messagecontext.LastRetry,
                    Origin = messagecontext.Origin,
                    Headers = messagecontext.Headers,
                    DateTimeUtc = messagecontext.DateTimeUtc,
                    Name = messagecontext.Route.Name,
                    Tracks = messagecontext.Tracks,
                    ContentId = messagecontext.ContentId,
                    Data = string.Empty
                };

                try
                {
                    storage.CreateMessage(messagecontext, messageentity);
                }
                catch (Exception)
                {
                    if (!_configuration.Storage.IgnoreExceptionOnSaveMessage)
                    {
                        throw;
                    }
                }
            }
            
            _router.Route(messagecontext);
        }
    }
}