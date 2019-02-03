using System;
using System.Text;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.StartupTask
{
    public class PublishSubscribeChannelCreator : AbstractStartupTask, IStartupTask
    {
        public PublishSubscribeChannelCreator(IComponentFactory factory, IConfiguration configuration, ILogger logger)
            : base(factory, configuration, logger)
        {
        }

        public void Run()
        {
            var errors = new StringBuilder();

            Logger.Log("Creating publish subscribe channels");

            var manager = Factory.Create<IChannelManager>(Configuration.ChannelManagerType);

            foreach (var channel in Configuration.Runtime.PublishSubscribeChannels)
            {

                if (channel.ConnectionStringValueFinderType != null)
                {
                    var finder = Factory.Create<IValueFinder>(channel.ConnectionStringValueFinderType);

                    var provider = channel.ConnectionStringProvider as Func<IValueFinder, string>;

                    channel.ConnectionString = provider?.Invoke(finder);

                    if (string.IsNullOrWhiteSpace(channel.ConnectionString))
                    {
                        var error = $"Empty connection string, publish subscribe channel {channel.Path}";

                        errors.AppendLine(error);

                        Logger.Log(error);

                        return;
                    }

                    if (string.IsNullOrWhiteSpace(channel.Path))
                    {
                        var error = $"Empty path, publish subscribe channel {channel.Path}";

                        errors.AppendLine(error);

                        Logger.Log(error);

                        return;
                    }

                    try
                    {
                        var created = manager.CreateIfNotExist(channel);

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
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new Exception(errors.ToString());
            }

            Logger.Log("Publish subscribe channels created");
        }
    }
}