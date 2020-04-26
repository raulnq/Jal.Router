using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IChannelOptionBuilder
    {
        void With(Action<IOptionBuilder> action);
    }
}