using System;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractChannel
    {
        private readonly string _channelname;

        protected readonly IComponentFactory Factory;

        protected readonly IConfiguration Configuration;

        private readonly IChannelPathBuilder _builder;

        protected AbstractChannel(string channelname, IComponentFactory factory, IConfiguration configuration, IChannelPathBuilder builder)
        {
            _channelname = channelname;
            Factory = factory;
            Configuration = configuration;
            _builder = builder;
        }

        public virtual string Send(MessageContext context, object message)
        {
            return string.Empty;
        }

        public virtual void Listen(Route route, Action<object>[] routeactions,  string channelpath)
        {

        }

        public object Reply(MessageContext context)
        {
            var channelpath = _builder.BuildReplyFromContext(context);

            Send(context);

            MessageContext outputcontext = null;

            var adapter = Factory.Create<IMessageAdapter>(Configuration.MessageAdapterType);

            try
            {
                outputcontext = string.IsNullOrWhiteSpace(context.ToReplySubscription) ? ReceiveOnQueue(context, adapter) : ReceiveOnTopic(context, adapter);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message {outputcontext?.Id} failed to arrived to {_channelname} channel {channelpath} {ex}");

                throw;
            }
            finally
            {
                Console.WriteLine($"Message {outputcontext?.Id} arrived to {_channelname} channel {channelpath}");
            }

            if (outputcontext != null)
            {
                return adapter.Deserialize(context.ContentAsString, context.ContentType);
            }

            return null;
        }

        public virtual MessageContext ReceiveOnQueue(MessageContext inputcontext, IMessageAdapter adapter)
        {
            return null;
        }

        public virtual MessageContext ReceiveOnTopic(MessageContext inputcontext, IMessageAdapter adapter)
        {
            return null;
        }

        public void Send(MessageContext context)
        {
            var channelpath = _builder.BuildFromContext(context);

            var id = string.Empty;

            try
            {
                var adapter = Factory.Create<IMessageAdapter>(Configuration.MessageAdapterType);

                var message = adapter.Write(context);

                id = Send(context, message);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message {id} failed to sent to {_channelname} channel {channelpath} {ex}");

                throw;
            }
            finally
            {
                Console.WriteLine($"Message {id} sent to {_channelname} channel {channelpath}");
            }
            
        }

        public void OnMessage(string channelpath, string messageid, Action routeaction, Action completeaction)
        {
            Console.WriteLine($"Message {messageid} arrived to {_channelname} channel {channelpath}");

            try
            {
                routeaction();

                completeaction();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message {messageid} failed to {_channelname} channel {channelpath} {ex}");
            }
            finally
            {
                Console.WriteLine($"Message {messageid} completed to {_channelname} channel {channelpath}");
            }
        }
    }
}