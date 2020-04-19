using Jal.Router.Interface;

namespace Jal.Router.Newtonsoft
{
    public static class RouterBuilderExtensions
    {
        public static void AddNewtonsoft(this IRouterBuilder builder)
        {
            builder.AddMessageSerializer<JsonMessageSerializer>();
        }
    }
}
