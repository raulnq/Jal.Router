using System;
using System.Threading;
using System.Threading.Tasks;

namespace Jal.Router.Interface.Management
{
    public interface IStartupTask
    {
        void Run();
    }
}