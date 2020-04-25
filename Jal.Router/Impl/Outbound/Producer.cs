using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class Producer : IProducer
    {
        public Producer(IComponentFactoryFacade factory, ILogger logger, ISenderContextLifecycle lifecycle)
        {
            _factory = factory;
            _logger = logger;
            _lifecycle = lifecycle;
        }

        private readonly IComponentFactoryFacade _factory;

        private readonly ISenderContextLifecycle _lifecycle;

        private readonly ILogger _logger;

        public async Task Produce(MessageContext context)
        {
            var id = string.Empty;

            try
            {
                var sendercontext = _lifecycle.Get(context.Channel);

                if (sendercontext == null)
                {
                    sendercontext = _lifecycle.Add(context.Channel);

                    if (sendercontext.Open())
                    {
                        _logger.Log($"Opening {sendercontext.Id}");
                    }
                }

                var message = await sendercontext.MessageAdapter.WritePhysicalMessage(context).ConfigureAwait(false);

                id = await sendercontext.Send(message).ConfigureAwait(false);

                if (sendercontext.ReaderChannel != null)
                {
                    MessageContext outputcontext = null;

                    try
                    {
                        outputcontext = await sendercontext.Read(context).ConfigureAwait(false);
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

                        context.ContentContext.SetResult(serializer.Deserialize(outputcontext.ContentContext.Data, outputcontext.ContentContext.Type));
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