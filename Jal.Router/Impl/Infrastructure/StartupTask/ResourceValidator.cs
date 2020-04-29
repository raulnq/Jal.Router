using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ResourceValidator : AbstractStartupTask, IStartupTask
    {
        public ResourceValidator(IComponentFactoryFacade factory, ILogger logger)
        : base(factory, logger)
        {
        }

        public Task Run()
        {
            var errors = new StringBuilder();

            Logger.Log("Validating resources");

            foreach (var resource in Factory.Configuration.Runtime.Resources)
            {
                if (string.IsNullOrWhiteSpace(resource.ConnectionString))
                {
                    var error = $"Empty connection string, {resource.ToString()} resource {resource.FullPath}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }

                if (string.IsNullOrWhiteSpace(resource.Path))
                {
                    var error = $"Empty path, {resource.ToString()} resource {resource.FullPath}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }

                if(resource.ChannelType == Model.ChannelType.SubscriptionToPublishSubscribe)
                {
                    if (string.IsNullOrWhiteSpace(resource.Subscription))
                    {
                        var error = $"Empty subscription, {resource.ToString()} resource {resource.FullPath}";

                        errors.AppendLine(error);

                        Logger.Log(error);
                    }

                    if (resource.Rules.Count == 0)
                    {
                        var error = $"Missing rules, {resource.ToString()} resource {resource.FullPath}";

                        errors.AppendLine(error);

                        Logger.Log(error);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            Logger.Log("Resources validated");

            return Task.CompletedTask;
        }
    }
}