using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IAndWaitReplyFromBuilder
    {
        void AndWaitReplyFromPointToPointChannel(string path, string connectionstring, int timeout = 60);
        void AndWaitReplyFromSubscriptionToPublishSubscribeChannel(string path, string subscription, string connectionstring, int timeout = 60);
    }
}