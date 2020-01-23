using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{

    public class SubscriptionToPublishSubscribeChannelResourceCreator : AbstractStartupTask, IStartupTask
    {
        public SubscriptionToPublishSubscribeChannelResourceCreator(IComponentFactoryGateway factory, ILogger logger) 
            : base(factory, logger)
        {
        }

        public async Task Run()
        {
            var errors = new StringBuilder();

            Logger.Log("Creating subscription to publish subscribe channels");

            var manager = Factory.CreateSubscriptionToPublishSubscribeChannelResourceManager();

            foreach (var channel in Factory.Configuration.Runtime.SubscriptionToPublishSubscribeChannelResources)
            {
                try
                {
                    var created = await manager.CreateIfNotExist(channel).ConfigureAwait(false);

                    if (created)
                    {
                        Logger.Log($"Created subscription {channel.Subscription} to publish subscribe channel {channel.Path}");
                    }
                    else
                    {
                        Logger.Log($"Subscription {channel.Subscription} to publish subscribe channel {channel.Path} already exists");
                    }
                }
                catch (Exception ex)
                {
                    var error = $"Exception subscription {channel.Subscription} to publish subscribe channel {channel.Path}: {ex}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            Logger.Log("Subscription to publish subscribe channels created");
        }
    }
}