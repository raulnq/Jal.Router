using System;

namespace Jal.Router.Interface
{
    public interface ILogger<in TMessage>
    {
        void Log(TMessage message, DateTime datetime);
    }

    public interface ILogger
    {
        void Log(string message);
    }
}