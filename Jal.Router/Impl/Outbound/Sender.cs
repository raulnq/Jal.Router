using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl.Outbound
{
    public class Sender : ISender
    {
        public Sender(IComponentFactoryGateway factory, IConfiguration configuration, ILogger logger)
        {
            _factory = factory;
            _configuration = configuration;
            _logger = logger;
        }

        private readonly IComponentFactoryGateway _factory;

        private readonly IConfiguration _configuration;

        private readonly ILogger _logger;

        public async Task Send(MessageContext context)
        {
            var id = string.Empty;

            try
            {
                var adapter = _factory.CreateMessageAdapter();

                var message = await adapter.WriteMetadataAndContent(context, context.EndPoint).ConfigureAwait(false);

                var metadata = _configuration.Runtime.SendersMetadata.FirstOrDefault(x => x.Channel.GetId() == context.Channel.GetId());

                if (metadata == null)
                {
                    metadata = DynamicEndpointLoader(context.Channel, context);

                    _configuration.Runtime.SendersMetadata.Add(metadata);
                }

                id = await metadata.Sender.Send(message).ConfigureAwait(false);

                if (metadata.Reader != null)
                {
                    MessageContext outputcontext = null;

                    try
                    {
                        outputcontext = await metadata.Reader.Read(context, adapter).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.Log($"Message {outputcontext?.IdentityContext.Id} failed to arrived to {context.Channel.ToString()} channel {context.EndPoint.Name}/{context.Channel.GetPath()} {ex}");

                        throw;
                    }
                    finally
                    {
                        _logger.Log($"Message {outputcontext?.IdentityContext.Id} arrived to {context.Channel.ToString()} channel {context.EndPoint.Name}/{context.Channel.GetPath()}");
                    }

                    if (outputcontext != null)
                    {
                        var serializer = _factory.CreateMessageSerializer();

                        context.Response = serializer.Deserialize(outputcontext.Content, outputcontext.ContentType);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Message {id} failed to sent to {context.Channel.ToString()} channel {context.EndPoint.Name}/{context.Channel.GetPath()}  {ex}");

                throw;
            }
            finally
            {
                _logger.Log($"Message {id} sent to {context.Channel.ToString()} channel {context.EndPoint.Name}/{context.Channel.GetPath()}");
            }
        }

        private SenderMetadata DynamicEndpointLoader(Channel channel, MessageContext context)
        {
            var sender = new SenderMetadata(channel);

            sender.Endpoints.Add(context.EndPoint);

            var senderchannel = _factory.CreateSenderChannel(sender.Channel.Type);

            if(senderchannel!=null)
            {
                senderchannel.Open(sender);

                sender.Sender = senderchannel;

                _logger.Log($"Opening {sender.Signature()}");
            }

            return sender;
        }
    }
}