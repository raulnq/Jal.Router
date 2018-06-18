using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractChannel
    {
        protected readonly string ChannelName;

        protected readonly IComponentFactory Factory;

        protected readonly IConfiguration Configuration;

        protected AbstractChannel(string channelName, IComponentFactory factory, IConfiguration configuration)
        {
            ChannelName = channelName;
            Factory = factory;
            Configuration = configuration;
        }

        public virtual string Send(Channel channel, object message)
        {
            return string.Empty;
        }

        public virtual void Listen(Channel channel, Action<object>[] actions, string channelpath)
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
                Console.WriteLine($"Message {outputcontext?.Id} failed to arrived to {ChannelName} channel {channelpath} {ex}");

                throw;
            }
            finally
            {
                Console.WriteLine($"Message {outputcontext?.Id} arrived to {ChannelName} channel {channelpath}");
            }

            if (outputcontext != null)
            {
                return adapter.Deserialize(outputcontext.ContentAsString, outputcontext.ContentType);
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

                var message = adapter.Write(context);

                id = Send(channel, message);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message {id} failed to sent to {ChannelName} channel {channelpath} {ex}");

                throw;
            }
            finally
            {
                Console.WriteLine($"Message {id} sent to {ChannelName} channel {channelpath}");
            }
            
        }

        public void OnMessage(string channelpath, string messageid, Action @action, Action completeaction)
        {
            Console.WriteLine($"Message {messageid} arrived to {ChannelName} channel {channelpath}");

            try
            {
                action();

                completeaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message {messageid} failed to {ChannelName} channel {channelpath} {ex}");
            }
            finally
            {
                Console.WriteLine($"Message {messageid} completed to {ChannelName} channel {channelpath}");
            }
        }

        public async Task OnMessageAsync(string channelpath, string messageid, Action @action, Func<Task> completeaction)
        {
            Console.WriteLine($"Message {messageid} arrived to {ChannelName} channel {channelpath}");

            try
            {
                action();

                await completeaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message {messageid} failed to {ChannelName} channel {channelpath} {ex}");
            }
            finally
            {
                Console.WriteLine($"Message {messageid} completed to {ChannelName} channel {channelpath}");
            }
        }
    }
}