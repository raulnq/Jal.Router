using System;
using System.Linq;
using System.Threading.Tasks;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{
    public class AzureServiceBus : AbstractChannel
    {
        private readonly AzureServiceBusChannelConnection _parameter;

        public AzureServiceBus(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider) : base(factory, logger)
        {
            _parameter = provider.Get<AzureServiceBusChannelConnection>();
        }

        protected AzureServiceBusChannelConnection Get(Channel channel)
        {
            if(channel.Properties.Any(x=>x.Key.StartsWith("connection_")))
            {
                return new AzureServiceBusChannelConnection(channel.Properties);
            }
            else
            {
                if(_parameter==null)
                {
                    return new AzureServiceBusChannelConnection();
                }
                else
                {
                    return _parameter;
                }
            }
        }

        protected SessionHandlerOptions CreateSessionOptions(ListenerContext listenercontext)
        {
            var connection = Get(listenercontext.Channel);

            Func<ExceptionReceivedEventArgs, Task> handler = args =>
            {
                var context = args.ExceptionReceivedContext;

                var exception = connection.LogClientException ? args.Exception.ToString() : args.Exception.Message;

                Logger.Log($"Listener {listenercontext.Channel.ToString()} failed, channel {listenercontext.Channel.FullPath} " +
                    $"Endpoint: {context.Endpoint} Entity Path: {context.EntityPath} " +
                    $"Executing Action: {context.Action}, {exception}");

                return Task.CompletedTask;
            };

            var options = new SessionHandlerOptions(handler) { AutoComplete = false };

            if (connection.MaxConcurrentPartitions > 0)
            {
                options.MaxConcurrentSessions = connection.MaxConcurrentPartitions;
            }
            if (connection.AutoRenewPartitionTimeoutInSeconds > 0)
            {
                options.MaxAutoRenewDuration = TimeSpan.FromSeconds(connection.AutoRenewPartitionTimeoutInSeconds);
            }
            if (connection.MessagePartitionTimeoutInSeconds > 0)
            {
                options.MessageWaitTimeout = TimeSpan.FromSeconds(connection.MessagePartitionTimeoutInSeconds);
            }
            return options;
        }

        protected MessageHandlerOptions CreateOptions(ListenerContext listenercontext)
        {
            var connection = Get(listenercontext.Channel);

            Func<ExceptionReceivedEventArgs, Task> handler = args =>
            {
                var context = args.ExceptionReceivedContext;

                var exception = connection.LogClientException ? args.Exception.ToString() : args.Exception.Message;

                Logger.Log($"Listener {listenercontext.Channel.ToString()} failed, channel {listenercontext.Channel.FullPath}" +
                    $" Endpoint: {context.Endpoint} Entity Path: {context.EntityPath} " +
                    $"Executing Action: {context.Action}, {exception}");

                return Task.CompletedTask;
            };

            var options = new MessageHandlerOptions(handler) { AutoComplete = false };

            if (connection.MaxConcurrentCalls > 0)
            {
                options.MaxConcurrentCalls = connection.MaxConcurrentCalls;
            }
            if (connection.AutoRenewTimeoutInMinutes > 0)
            {
                options.MaxAutoRenewDuration = TimeSpan.FromMinutes(connection.AutoRenewTimeoutInMinutes);
            }
            return options;
        }
    }
}