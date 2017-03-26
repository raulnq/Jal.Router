using System;
using Jal.Router.Impl;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class MessageHandler : IMessageHandler<Message>
    {
        public void Handle(Message message)
        {
            Console.WriteLine("Sender"+ message.Name);
        }

        public void Handle(Message message, Response response)
        {
            throw new NotImplementedException();
        }
    }

    public interface IMessageHandler<in T>
    {
        void Handle(T message);

        void Handle(T message, Response response);
    }
}