using System;
using Jal.Router.Impl;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class MessageHandler : AbstractMessageHandler<Request>
    {
        public override void Handle(Request message)
        {
            Console.WriteLine("MessageSender");
        }
    }
}