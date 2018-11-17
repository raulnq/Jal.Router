using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl
{
    public abstract class AbstractChannel
    {
        protected readonly string Name;

        protected readonly IComponentFactory Factory;

        protected readonly IConfiguration Configuration;

        protected readonly ILogger Logger;

        protected AbstractChannel(string name, IComponentFactory factory, IConfiguration configuration, ILogger logger)
        {
            Name = name;
            Factory = factory;
            Configuration = configuration;
            Logger = logger;
        }

        public virtual string Send(Channel channel, object message)
        {
            return string.Empty;
        }

        public virtual void Listen(ListenerMetadata metadata)
        {

        }

        public object Reply(Channel channel, MessageContext context, string channelpath)
        {
            Send(channel, context, channelpath);

            MessageContext outputcontext = null;

            var adapter = Factory.Create<IMessageAdapter>(Configuration.MessageAdapterType);

            try
            {
                outputcontext = string.IsNullOrWhiteSpace(channel.ToReplySubscription) ? ReceiveOnQueue(channel, context, adapter) : ReceiveOnTopic(channel, context, adapter);
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {outputcontext?.Identity.Id} failed to arrived to {Name} channel {channelpath} {ex}");

                throw;
            }
            finally
            {
                Logger.Log($"Message {outputcontext?.Identity.Id} arrived to {Name} channel {channelpath}");
            }

            if (outputcontext != null)
            {
                return adapter.Deserialize(outputcontext.Content, outputcontext.ContentType);
            }

            return null;
        }

        public virtual MessageContext ReceiveOnQueue(Channel channel, MessageContext context, IMessageAdapter adapter)
        {
            return null;
        }

        public virtual MessageContext ReceiveOnTopic(Channel channel, MessageContext context, IMessageAdapter adapter)
        {
            return null;
        }

        public void Send(Channel channel, MessageContext context, string channelpath)
        {
            var id = string.Empty;

            try
            {
                var adapter = Factory.Create<IMessageAdapter>(Configuration.MessageAdapterType);

                var message = adapter.Write(context, context.EndPoint.UseClaimCheck);

                id = Send(channel, message);

            }
            catch (Exception ex)
            {
                Logger.Log($"Message {id} failed to sent to {Name} channel {channelpath} {ex}");

                throw;
            }
            finally
            {
                Logger.Log($"Message {id} sent to {Name} channel {channelpath}");
            }
            
        }

        public void OnMessage(string channelpath, string messageid, Action @action, Action completeaction)
        {
            Logger.Log($"Message {messageid} arrived to {Name} channel {channelpath}");

            try
            {
                action();

                completeaction();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {messageid} failed to {Name} channel {channelpath} {ex}");
            }
            finally
            {
                Logger.Log($"Message {messageid} completed to {Name} channel {channelpath}");
            }
        }

        public async Task OnMessageAsync(string channelpath, string messageid, Action @action, Func<Task> completeaction)
        {
            Logger.Log($"Message {messageid} arrived to {Name} channel {channelpath}");

            try
            {
                action();

                await completeaction();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {messageid} failed to {Name} channel {channelpath} {ex}");
            }
            finally
            {
                Logger.Log($"Message {messageid} completed to {Name} channel {channelpath}");
            }
        }
    }
}