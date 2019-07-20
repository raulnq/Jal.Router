using System.Threading;

namespace Jal.Router.Interface
{
    public interface IShutdownWatcher
    {
        void Start(CancellationTokenSource tokensource);

        void Stop();
    }
}