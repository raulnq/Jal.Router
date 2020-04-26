using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IAndWaitReplyFromEndPointBuilder
    {
        void AndWaitReplyFromPointToPointChannel(string path, string connectionstring, int timeout = 60, Type adapter = null, Type type = null);
        void AndWaitReplyFromSubscriptionToPublishSubscribeChannel(string path, string subscription, string connectionstring, int timeout = 60, Type adapter = null, Type type = null);
    }
}