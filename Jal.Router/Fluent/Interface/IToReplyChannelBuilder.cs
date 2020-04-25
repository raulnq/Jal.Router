using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IToReplyChannelBuilder
    {
        IAndWaitReplyFromEndPointBuilder AddPointToPointChannel(string connectionstring, string path);

    }
}