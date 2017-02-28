using System;
using Jal.Router.Impl;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class MessageSender : AbstractMessageSender<Request>
    {
        public override void Send(Request message)
        {
            Console.WriteLine("MessageSender");
        }
    }
}