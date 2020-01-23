using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class PublishSubscribeChannelResourceCreator : AbstractStartupTask, IStartupTask
    {
        public PublishSubscribeChannelResourceCreator(IComponentFactoryGateway factory, ILogger logger)
            : base(factory, logger)
        {
        }

        public async Task Run()
        {
            var errors = new StringBuilder();

            Logger.Log("Creating publish subscribe channels");

            var manager = Factory.CreatePublishSubscribeChannelResourceManager();

            foreach (var channel in Factory.Configuration.Runtime.PublishSubscribeChannelResources)
            {
                try
                {
                    var created = await manager.CreateIfNotExist(channel).ConfigureAwait(false);

                    if (created)
                    {
                        Logger.Log($"Created {channel.Path} publish subscribe channel");
                    }
                    else
                    {
                        Logger.Log($"Publish subscribe channel {channel.Path} already exists");
                    }
                }
                catch (Exception ex)
                {
                    var error = $"Exception {channel.Path} publish subscribe channel: {ex}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            Logger.Log("Publish subscribe channels created");
        }
    }
}