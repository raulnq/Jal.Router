using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Logger
{
    public static class RouterBuilderExtensions
    {
        public static void AddCommonLogging(this IRouterBuilder builder)
        {
            builder.AddMiddleware<BusLogger>();

            builder.AddMiddleware<RouterLogger>();

            builder.AddLogger<BeatLogger, Beat>();
        }
    }
}
