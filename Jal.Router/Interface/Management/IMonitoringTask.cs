using System;
using System.Threading.Tasks;

namespace Jal.Router.Interface.Management
{
    public interface IMonitoringTask
    {
        Task Run(DateTime datetime);
    }
}