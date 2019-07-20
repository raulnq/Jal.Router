using System;
using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IMonitoringTask
    {
        Task Run(DateTime datetime);
    }
}