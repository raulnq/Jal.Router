using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class PublishSubscribeChannelResourceValidator : AbstractStartupTask, IStartupTask
    {
        public PublishSubscribeChannelResourceValidator(IComponentFactoryGateway factory, ILogger logger)
        : base(factory, logger)
        {
        }

        public Task Run()
        {
            var errors = new StringBuilder();

            Logger.Log("Validating publish subscribe channel resources");

            foreach (var channel in Factory.Configuration.Runtime.PublishSubscribeChannelResources)
            {
                if (string.IsNullOrWhiteSpace(channel.ConnectionString))
                {
                    var error = $"Empty connection string, publish subscribe channel {channel.Path}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }

                if (string.IsNullOrWhiteSpace(channel.Path))
                {
                    var error = $"Empty path, publish subscribe channel {channel.Path}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            Logger.Log("Publish subscribe channel resources validated");

            return Task.CompletedTask;
        }

    }
}