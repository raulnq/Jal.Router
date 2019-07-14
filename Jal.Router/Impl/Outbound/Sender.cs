using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

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

                var sendercontext = _configuration.Runtime.SenderContexts.FirstOrDefault(x => x.Channel.Id == context.Channel.Id);

                if (sendercontext == null)
                {
                    sendercontext = DynamicEndpointLoader(context.Channel, context);

                    _configuration.Runtime.SenderContexts.Add(sendercontext);
                }

                id = await sendercontext.SenderChannel.Send(sendercontext, message).ConfigureAwait(false);

                if (sendercontext.ReaderChannel != null)
                {
                    MessageContext outputcontext = null;

                    try
                    {
                        outputcontext = await sendercontext.ReaderChannel.Read(sendercontext, context, adapter).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.Log($"Message {outputcontext?.Id} failed to arrived to {context.Channel.ToString()} channel {context.EndPoint.Name}/{context.Channel.FullPath} {ex}");

                        throw;
                    }
                    finally
                    {
                        _logger.Log($"Message {outputcontext?.Id} arrived to {context.Channel.ToString()} channel {context.EndPoint.Name}/{context.Channel.FullPath}");
                    }

                    if (outputcontext != null)
                    {
                        var serializer = _factory.CreateMessageSerializer();

                        context.ContentContext.UpdateResponse(serializer.Deserialize(outputcontext.ContentContext.Data, outputcontext.ContentContext.Type));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Message {id} failed to sent to {context.Channel.ToString()} channel {context.EndPoint.Name}/{context.Channel.FullPath}  {ex}");

                throw;
            }
            finally
            {
                _logger.Log($"Message {id} sent to {context.Channel.ToString()} channel {context.EndPoint.Name}/{context.Channel.FullPath}");
            }
        }

        private SenderContext DynamicEndpointLoader(Channel channel, MessageContext context)
        {
            var sender = new SenderContext(channel);

            sender.Endpoints.Add(context.EndPoint);

            var senderchannel = _factory.CreateSenderChannel(sender.Channel.Type);

            if(senderchannel!=null)
            {
                senderchannel.Open(sender);

                sender.UpdateSenderChannel(senderchannel);

                _logger.Log($"Opening {sender.Id}");
            }

            return sender;
        }
    }
}