using System;
using Jal.Router.Impl;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class OtherMessageHandler : IMessageHandler<Message>
    {
        public void Handle(Message message, Data data)
        {
            Console.WriteLine("Other Sender"+ message.Name);
        }
    }
}