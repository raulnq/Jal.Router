using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IToChannelBuilder
    {
        void AddPointToPointChannel(string connectionstring, string path, Type adapter = null, Type type = null);

        void AddPublishSubscribeChannel(string connectionstring, string path, Type adapter = null, Type type = null);
    }
}