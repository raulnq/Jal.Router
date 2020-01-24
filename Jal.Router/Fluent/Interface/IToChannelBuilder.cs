using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IToChannelBuilder
    {
        void AddPointToPointChannel(string connectionstring, string path);

        void AddPublishSubscribeChannel(string connectionstring, string path);
    }
}