using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class EndpointValidator : AbstractStartupTask, IStartupTask
    {
        private readonly IChannelValidator _validator;
        public EndpointValidator(IComponentFactoryGateway factory, ILogger logger, IChannelValidator validator)
            :base(factory, logger)
        {
            _validator = validator;
        }

        public Task Run()
        {
            Logger.Log("Validating endpoints");

            var errors = new StringBuilder();

            foreach (var endpoint in Factory.Configuration.Runtime.EndPoints)
            {
                if (endpoint.Channels.Any())
                {
                    foreach (var channel in endpoint.Channels)
                    {
                        var result = _validator.Validate(channel, "Endpoint", endpoint.Name);

                        if (!string.IsNullOrWhiteSpace(result))
                        {
                            errors.AppendLine(result);
                        }
                    }
                }
                else
                {
                    var error = $"Missing channels, Endpoint {endpoint.Name}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            Logger.Log("Endpoints validated");

            return Task.CompletedTask;
        }
    }
}