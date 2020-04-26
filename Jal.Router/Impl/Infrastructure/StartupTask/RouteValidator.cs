using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class RouteValidator : AbstractStartupTask, IStartupTask
    {
        private readonly IChannelValidator _validator;
        public RouteValidator(IComponentFactoryFacade factory, ILogger logger, IChannelValidator validator)
            : base(factory, logger)
        {
            _validator = validator;
        }

        public Task Run()
        {
            Logger.Log("Validating routes");

            var errors = new StringBuilder();

            foreach (var route in Factory.Configuration.Runtime.Routes)
            {
                if (route.Channels.Any())
                {
                    foreach (var channel in route.Channels)
                    {
                        var result = _validator.Validate(channel, "Route", route.Name);

                        if(!string.IsNullOrWhiteSpace(result))
                        {
                            errors.AppendLine(result);
                        }
                    }
                }
                else
                {
                    var error = $"Missing channels, Handler {route.Name}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }

            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            Logger.Log("Routes validated");

            return Task.CompletedTask;
        }
    }
}