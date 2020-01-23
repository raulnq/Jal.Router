using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class SubscriptionToPublishSubscribeChannelResourceValidator : AbstractStartupTask, IStartupTask
    {
        public SubscriptionToPublishSubscribeChannelResourceValidator(IComponentFactoryGateway factory, ILogger logger)
        : base(factory, logger)
        {
        }

        public Task Run()
        {
            var errors = new StringBuilder();

            Logger.Log("Validating subscription to publish subscriber channel resources");

            foreach (var channel in Factory.Configuration.Runtime.SubscriptionToPublishSubscribeChannelResources)
            {
                if (string.IsNullOrWhiteSpace(channel.ConnectionString))
                {
                    var error = $"Empty connection string, subscription {channel.Subscription} to publish subscribe channel {channel.Path}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }

                if (string.IsNullOrWhiteSpace(channel.Path))
                {
                    var error = $"Empty path, subscription {channel.Subscription} to publish subscribe channel {channel.Path}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }

                if (string.IsNullOrWhiteSpace(channel.Subscription))
                {
                    var error = $"Empty subscription, subscription {channel.Subscription} to publish subscribe channel {channel.Path}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }

                if (channel.Rules.Count == 0)
                {
                    var error = $"Missing rules, subscription {channel.Subscription}  to publish subscribe channel {channel.Path}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            Logger.Log("Subscription to publish subscriber Channel resources validated");

            return Task.CompletedTask;
        }
    }
}