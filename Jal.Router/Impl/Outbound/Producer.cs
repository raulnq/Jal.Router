using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class Producer : IProducer
    {
        public Producer(IComponentFactoryGateway factory, IConfiguration configuration, ILogger logger, ISenderContextLoader loader)
        {
            _factory = factory;
            _configuration = configuration;
            _logger = logger;
            _loader = loader;
        }

        private readonly IComponentFactoryGateway _factory;

        private readonly IConfiguration _configuration;

        private readonly ISenderContextLoader _loader;

        private readonly ILogger _logger;

        public async Task Send(MessageContext context)
        {
            var id = string.Empty;

            try
            {
                var adapter = _factory.CreateMessageAdapter();

                var message = await adapter.WritePhysicalMessage(context).ConfigureAwait(false);

                var sendercontext = _configuration.Runtime.SenderContexts.FirstOrDefault(x => x.Channel.Id == context.Channel.Id);

                if (sendercontext == null)
                {
                    sendercontext = _loader.Create(context.Channel);

                    _configuration.Runtime.SenderContexts.Add(sendercontext);

                    sendercontext.Endpoints.Add(context.EndPoint);

                    _loader.Open(sendercontext);
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
    }
}