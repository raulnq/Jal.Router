using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class PointToPointChannelResourceCreator : AbstractStartupTask, IStartupTask
    {
        public PointToPointChannelResourceCreator(IComponentFactoryGateway factory, ILogger logger) 
            : base(factory, logger)
        {

        }

        public async Task Run()
        {
            var errors = new StringBuilder();

            Logger.Log("Creating point to point channels");

            var manager = Factory.CreatePointToPointChannelResourceManager();

            foreach (var channel in Factory.Configuration.Runtime.PointToPointChannelResources)
            {
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