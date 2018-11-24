using System;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Inbound.Sagas;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound.Sagas;

namespace Jal.Router.Impl.Inbound.Middleware
{
    public class MessageHandler : IMiddleware<MessageContext>
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

        public void Execute(Context<MessageContext> context, Action<Context<MessageContext>> next)
        {
            var messagecontext = context.Data;

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