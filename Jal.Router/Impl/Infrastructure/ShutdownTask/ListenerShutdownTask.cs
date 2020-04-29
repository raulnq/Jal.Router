using Jal.Router.Interface;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class ListenerShutdownTask : IShutdownTask
    {
        private readonly IConfiguration _configuration;

        public ListenerShutdownTask(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Run()
        {
            foreach (var context in _configuration.Runtime.ListenerContexts)
            {
                await context.Close().ConfigureAwait(false);
            }
        }
    }
}