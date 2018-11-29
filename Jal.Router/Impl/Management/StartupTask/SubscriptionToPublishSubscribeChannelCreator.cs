using System;
using System.Linq;
using System.Text;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.StartupTask
{

    public class SubscriptionToPublishSubscribeChannelCreator : AbstractStartupTask, IStartupTask
    {
        public SubscriptionToPublishSubscribeChannelCreator(IComponentFactory factory, IConfiguration configuration, ILogger logger) 
            : base(factory, configuration, logger)
        {
        }

        public void Run()
        {
            var errors = new StringBuilder();

            Logger.Log("Creating subscription to publish subscriber channels");

            var manager = Factory.Create<IChannelManager>(Configuration.ChannelManagerType);

            foreach (var channel in Configuration.Runtime.SubscriptionToPublishSubscribeChannels)
            {
                if(channel.ConnectionStringValueFinderType != null)
                {
                    var finder = Factory.Create<IValueFinder>(channel.ConnectionStringValueFinderType);

                    var provider = channel.ConnectionStringProvider as Func<IValueFinder, string>;

                    channel.ConnectionString = provider?.Invoke(finder);

                    if (string.IsNullOrWhiteSpace(channel.ConnectionString))
                    {
                        var error = $"Empty connection string, subscription {channel.Subscription} to publish subscriber channel {channel.Path}";

                        errors.AppendLine(error);

                        Logger.Log(error);

                        return;
                    }

                    if (string.IsNullOrWhiteSpace(channel.Path))
                    {
                        var error = $"Empty path, subscription {channel.Subscription} to publish subscriber channel {channel.Path}";

                        errors.AppendLine(error);

                        Logger.Log(error);

                        return;
                    }

                    if (string.IsNullOrWhiteSpace(channel.Subscription))
                    {
                        var error = $"Empty subscription, subscription {channel.Subscription} to publish subscriber channel {channel.Path}";

                        errors.AppendLine(error);

                        Logger.Log(error);

                        return;
                    }

                    if (channel.Rules.Count == 0)
                    {
                        var error = $"Missing rules, subscription {channel.Subscription}  to publish subscriber channel {channel.Path}";

                        errors.AppendLine(error);

                        Logger.Log(error);

                        return;
                    }

                    try
                    {
                        var created = manager.CreateIfNotExist(channel);

                        if (created)
                        {
                            Logger.Log($"Created subscription {channel.Subscription} to publish subscriber channel {channel.Path}");
                        }
                        else
                        {
                            Logger.Log($"Subscription {channel.Subscription} to publish subscriber channel {channel.Path} already exists");
                        }
                    }
                    catch (Exception ex)
                    {
                        var error = $"Exception subscription {channel.Subscription} to publish subscriber channel {channel.Path}: {ex}";

                        errors.AppendLine(error);

                        Logger.Log(error);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            Logger.Log("Subscription to publish subscriber channels created");
        }
    }
}