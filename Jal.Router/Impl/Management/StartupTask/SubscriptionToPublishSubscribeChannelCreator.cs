using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.StartupTask
{

    public class SubscriptionToPublishSubscribeChannelCreator : AbstractStartupTask, IStartupTask
    {
        public SubscriptionToPublishSubscribeChannelCreator(IComponentFactoryGateway factory, ILogger logger) 
            : base(factory, logger)
        {
        }

        public async Task Run()
        {
            var errors = new StringBuilder();

            Logger.Log("Creating subscription to publish subscribe channels");

            var manager = Factory.CreateChannelManager();

            foreach (var channel in Factory.Configuration.Runtime.SubscriptionToPublishSubscribeChannels)
            {
                if(channel.ConnectionStringValueFinderType != null && channel.ConnectionStringProvider!=null)
                {
                    var finder = Factory.CreateValueFinder(channel.ConnectionStringValueFinderType);

                    channel.ConnectionString = channel.ConnectionStringProvider(finder);
                }

                if (string.IsNullOrWhiteSpace(channel.ConnectionString))
                {
                    var error = $"Empty connection string, subscription {channel.Subscription} to publish subscribe channel {channel.Path}";

                    errors.AppendLine(error);

                    Logger.Log(error);

                    break;
                }

                if (string.IsNullOrWhiteSpace(channel.Path))
                {
                    var error = $"Empty path, subscription {channel.Subscription} to publish subscribe channel {channel.Path}";

                    errors.AppendLine(error);

                    Logger.Log(error);

                    break;
                }

                if (string.IsNullOrWhiteSpace(channel.Subscription))
                {
                    var error = $"Empty subscription, subscription {channel.Subscription} to publish subscribe channel {channel.Path}";

                    errors.AppendLine(error);

                    Logger.Log(error);

                    break;
                }

                if (channel.Rules.Count == 0)
                {
                    var error = $"Missing rules, subscription {channel.Subscription}  to publish subscribe channel {channel.Path}";

                    errors.AppendLine(error);

                    Logger.Log(error);

                    break;
                }

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