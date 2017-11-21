using System;

namespace Jal.Router.Interface
{
    public interface ILogger<in TInfo>
    {
        void Log(TInfo info, DateTime datetime);
    }
}