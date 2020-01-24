using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class PointToPointChannelResourceValidator : AbstractStartupTask, IStartupTask
    {
        public PointToPointChannelResourceValidator(IComponentFactoryGateway factory, ILogger logger)
        : base(factory, logger)
        {
        }

        public Task Run()
        {
            var errors = new StringBuilder();

            Logger.Log("Validating point to point channel resources");

            foreach (var channel in Factory.Configuration.Runtime.PointToPointChannelResources)
            {
                if (string.IsNullOrWhiteSpace(channel.ConnectionString))
                {
                    var error = $"Empty connection string, point to point channel {channel.Path}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }

                if (string.IsNullOrWhiteSpace(channel.Path))
                {
                    var error = $"Empty path, point to point channel {channel.Path}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            Logger.Log("Point to point channel resources validated");

            return Task.CompletedTask;
        }
    }
}