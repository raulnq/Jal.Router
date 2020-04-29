using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class Producer : IProducer
    {
        public Producer(ILogger logger, ISenderContextLifecycle lifecycle)
        {
            _logger = logger;
            _lifecycle = lifecycle;
        }

        private readonly ISenderContextLifecycle _lifecycle;

        private readonly ILogger _logger;

        public async Task Produce(MessageContext context)
        {
            var id = string.Empty;

            try
            {
                var sendercontext = _lifecycle.Get(context.Channel);

                var message = await sendercontext.Write(context).ConfigureAwait(false);

                id = await sendercontext.Send(message).ConfigureAwait(false);

                if (sendercontext.Channel.ReplyType != ReplyType.None)
                {
                    MessageContext outputcontext = null;

                    try
                    {
                        outputcontext = await sendercontext.Read(context).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.Log($"Message {outputcontext?.Id} failed to arrived to {context.Channel.ToString()} channel {context.Channel.FullPath} endpoint {context.EndPoint?.Name} {ex}");

                        throw;
                    }
                    finally
                    {
                        _logger.Log($"Message {outputcontext?.Id} arrived to {context.Channel.ToString()} channel {context.Channel.FullPath} endpoint {context.EndPoint?.Name}");
                    }

                    if (outputcontext != null)
                    {
                        context.ContentContext.ReplyData = sendercontext.MessageSerializer.Deserialize(outputcontext.ContentContext.Data, context.EndPoint.ReplyContentType);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Message {id} failed to sent to {context.Channel.ToString()} channel {context.Channel.FullPath} endpoint {context.EndPoint?.Name} {ex}");

                throw;
            }
            finally
            {
                _logger.Log($"Message {id} sent to {context.Channel.ToString()} channel {context.Channel.FullPath} endpoint {context.EndPoint?.Name}");
            }
        }
    }
}