using System;
using Jal.Router.Impl;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class OtherMessageHandler : IMessageHandler<Message>
    {
        public void Handle(Message message)
        {
            Console.WriteLine("Other Sender"+ message.Name);
        }

        public void Handle(Message message, Response response)
        {
            Console.WriteLine("Other Sender Response"+ message.Name);
        }
    }
}