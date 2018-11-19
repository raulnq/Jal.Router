using System;
using System.Text;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.StartupTask
{
    public class PointToPointChannelCreator : AbstractStartupTask, IStartupTask
    {
        public PointToPointChannelCreator(IComponentFactory factory, IConfiguration configuration, ILogger logger) 
            : base(factory, configuration, logger)
        {
        }

        public void Run()
        {
            var errors = new StringBuilder();

            Logger.Log("Creating point to point channels");

            var manager = Factory.Create<IChannelManager>(Configuration.ChannelManagerType);

            foreach (var channel in Configuration.Runtime.PointToPointChannels)
            {
                if (channel.ConnectionStringValueFinderType != null)
                {
                    var finder = Factory.Create<IValueFinder>(channel.ConnectionStringValueFinderType);

                    var provider = channel.ConnectionStringProvider as Func<IValueFinder, string>;

                    channel.ConnectionString = provider?.Invoke(finder);

                    if (string.IsNullOrWhiteSpace(channel.ConnectionString))
                    {
                        var error = $"Empty connection string, point to point channel {channel.Path}";

                        errors.AppendLine(error);

                        Logger.Log(error);

                        return;
                    }

                    if (string.IsNullOrWhiteSpace(channel.Path))
                    {
                        var error = $"Empty path, point to point channel {channel.Path}";

                        errors.AppendLine(error);

                        Logger.Log(error);

                        return;
                    }

                    try
                    {
                        var created = manager.CreateIfNotExist(channel);

                        if (created)
                        {
                            Logger.Log($"Created {channel.Path} point to point channel");
                        }
                        else
                        {
                            Logger.Log($"Point to point channel {channel.Path} already exists");
                        }
                    }
                    catch (Exception ex)
                    {
                        var error = $"Exception {channel.Path} point to point channel: {ex}";

                        errors.AppendLine(error);

                        Logger.Log(error);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new Exception(errors.ToString());
            }

            Logger.Log("Point to point channels created");
        }
    }
}