using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;

namespace Jal.Router.Impl.StartupTask
{
    public class PointToPointChannelCreator : AbstractStartupTask, IStartupTask
    {
        public PointToPointChannelCreator(IComponentFactoryGateway factory, ILogger logger) 
            : base(factory, logger)
        {
        }

        public async Task Run()
        {
            var errors = new StringBuilder();

            Logger.Log("Creating point to point channels");

            var manager = Factory.CreateChannelManager();

            foreach (var channel in Factory.Configuration.Runtime.PointToPointChannels)
            {
                if (channel.ConnectionStringValueFinderType != null && channel.ConnectionStringProvider!=null)
                {
                    var finder = Factory.CreateValueFinder(channel.ConnectionStringValueFinderType);

                    channel.ConnectionString = channel.ConnectionStringProvider(finder);
                }

                if (string.IsNullOrWhiteSpace(channel.ConnectionString))
                {
                    var error = $"Empty connection string, point to point channel {channel.Path}";

                    errors.AppendLine(error);

                    Logger.Log(error);

                    break;
                }

                if (string.IsNullOrWhiteSpace(channel.Path))
                {
                    var error = $"Empty path, point to point channel {channel.Path}";

                    errors.AppendLine(error);

                    Logger.Log(error);

                    break;
                }

                try
                {
                    var created = await manager.CreateIfNotExist(channel).ConfigureAwait(false);

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

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            Logger.Log("Point to point channels created");
        }
    }
}