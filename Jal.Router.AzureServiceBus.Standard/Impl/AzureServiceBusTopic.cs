using System;
using System.Threading.Tasks;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace Jal.Router.AzureServiceBus.Standard.Impl
{

    public class AzureServiceBusTopic : AbstractChannel, IPublishSubscribeChannel
    {
        private TopicClient _topicclient;

        public bool IsActive(SenderContext sendercontext)
        {
            return !_topicclient.IsClosedOrClosing;
        }

        public Task Close(SenderContext sendercontext)
        {
            return _topicclient.CloseAsync();
        }

        public void Open(SenderContext sendercontext)
        {
            _topicclient = new TopicClient(sendercontext.Channel.ConnectionString, sendercontext.Channel.Path);

            if (_parameter.TimeoutInSeconds > 0)
            {
                _topicclient.ServiceBusConnection.OperationTimeout = TimeSpan.FromSeconds(_parameter.TimeoutInSeconds);
            }
        }

        public async Task<string> Send(SenderContext sendercontext, object message)
        {
            var sbmessage = message as Microsoft.Azure.ServiceBus.Message;

            if (sbmessage != null)
            {
                await _topicclient.SendAsync(sbmessage).ConfigureAwait(false);

                return sbmessage.MessageId;
            }

            return string.Empty;
        }

        public async Task<bool> CreateIfNotExist(Channel channel)
        {
            var client = new ManagementClient(channel.ConnectionString);

            if (!await client.TopicExistsAsync(channel.Path).ConfigureAwait(false))
            {
                var description = new TopicDescription(channel.Path)
                {
                    SupportOrdering = true
                };

                var messagettl = 14;

                if (channel.Properties.ContainsKey(AzureServiceBusChannelProperties._DefaultMessageTtlInDays))
                {
                    messagettl = Convert.ToInt32(channel.Properties[AzureServiceBusChannelProperties._DefaultMessageTtlInDays]);
                }

                description.DefaultMessageTimeToLive = TimeSpan.FromDays(messagettl);

                if (channel.Properties.ContainsKey(AzureServiceBusChannelProperties._DuplicateMessageDetectionInMinutes))
                {
                    var duplicatemessagedetectioninminutes = Convert.ToInt32(channel.Properties[AzureServiceBusChannelProperties._DuplicateMessageDetectionInMinutes]);
                    description.RequiresDuplicateDetection = true;
                    description.DuplicateDetectionHistoryTimeWindow = TimeSpan.FromMinutes(duplicatemessagedetectioninminutes);
                }

                if (channel.Properties.ContainsKey(AzureServiceBusChannelProperties._PartitioningEnabled))
                {
                    description.EnablePartitioning = true;
                }

                if (channel.Properties.ContainsKey(AzureServiceBusChannelProperties._ExpressMessageEnabled))
                {

                }

                await client.CreateTopicAsync(description).ConfigureAwait(false);

                return true;
            }

            return false;
        }

        public async Task<Statistic> GetStatistic(Channel channel)
        {
            var client = new ManagementClient(channel.ConnectionString);

            if (await client.TopicExistsAsync(channel.Path).ConfigureAwait(false))
            {
                var info = await client.GetTopicRuntimeInfoAsync(channel.Path).ConfigureAwait(false);

                var statistics = new Statistic(channel.Path);

                statistics.Properties.Add("DeadLetterMessageCount", info.MessageCountDetails.DeadLetterMessageCount.ToString());

                statistics.Properties.Add("ActiveMessageCount", info.MessageCountDetails.ActiveMessageCount.ToString());

                statistics.Properties.Add("ScheduledMessageCount", info.MessageCountDetails.ScheduledMessageCount.ToString());

                statistics.Properties.Add("CurrentSizeInBytes", info.SizeInBytes.ToString());

                return statistics;
            }

            return null;
        }

        public async Task<bool> DeleteIfExist(Channel channel)
        {
            var client = new ManagementClient(channel.ConnectionString);

            if (await client.TopicExistsAsync(channel.Path).ConfigureAwait(false))
            {
                await client.DeleteTopicAsync(channel.Path).ConfigureAwait(false);

                return true;
            }

            return false;
        }

        public async Task<MessageContext> Read(SenderContext sendercontext, MessageContext context, IMessageAdapter adapter)
        {
            var client = default(SessionClient);

            if (sendercontext.Channel.ReplyChannel.ChannelType == ChannelType.PointToPoint)
            {
                client = new SessionClient(sendercontext.Channel.ReplyChannel.ConnectionString, sendercontext.Channel.ReplyChannel.Path);
            }
            else
            {
                var entity = EntityNameHelper.FormatSubscriptionPath(sendercontext.Channel.ReplyChannel.Path, sendercontext.Channel.ReplyChannel.Subscription);

                client = new SessionClient(sendercontext.Channel.ReplyChannel.ConnectionString, entity);
            }

            var messagesession = await client.AcceptMessageSessionAsync(context.TracingContext.ReplyToRequestId).ConfigureAwait(false);

            var message = sendercontext.Channel.ReplyTimeOut != 0 ?
                await messagesession.ReceiveAsync(TimeSpan.FromSeconds(sendercontext.Channel.ReplyTimeOut)).ConfigureAwait(false) :
                await messagesession.ReceiveAsync().ConfigureAwait(false);

            MessageContext outputcontext = null;

            if (message != null)
            {
                outputcontext = await adapter.ReadFromPhysicalMessage(message, sendercontext).ConfigureAwait(false);

                await messagesession.CompleteAsync(message.SystemProperties.LockToken).ConfigureAwait(false);
            }

            await messagesession.CloseAsync().ConfigureAwait(false);

            await client.CloseAsync().ConfigureAwait(false);

            return outputcontext;
        }

        private readonly AzureServiceBusChannelConnection _parameter;

        public AzureServiceBusTopic(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider)
            : base(factory, logger)
        {
            _parameter = provider.Get<AzureServiceBusChannelConnection>();
        }
    }
}