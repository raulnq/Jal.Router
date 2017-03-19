using System;
using Jal.Router.Impl;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class OtherMessageHandler : IMessageHandler<Request>
    {
        public void Handle(Request message)
        {
            Console.WriteLine("Other Sender"+ message.Name);
        }

        public void Handle(Request message, Response response)
        {
            Console.WriteLine("Other Sender Response"+ message.Name);
        }
    }
}