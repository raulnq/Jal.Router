using System;
using Jal.Router.Impl;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class OtherMessageHandler : IMessageHandler<Message>
    {
        private readonly IBus _bus;

        public OtherMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(Message message, Data data)
        {
            Console.WriteLine($"Other Sender {message.Name}");

            _bus.Send(message, new Options() {EndPointName = "route1" });

            //throw new ApplicationException();
        }
    }

    public class SagaInput1HandlerMessageHandler : IMessageSagaHandler<Message>
    {
        private readonly IBus _bus;

        public SagaInput1HandlerMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(MessageContext context, Message message, Data data)
        {
            Console.WriteLine($"SagaInput1HandlerMessageHandler Name {message.Name}");
            data.Status = "SagaInput1HandlerMessageHandler";
            //throw new Exception("error");
            _bus.Publish(message, new Options() {EndPointName = "SagaInputTopicHandlerMessageHandler", SagaContext = context.SagaContext});
        }
    }

    public class SagaInputTopicHandlerMessageHandler  : IMessageSagaHandler<Message>
    {
        private readonly IBus _bus;

        public SagaInputTopicHandlerMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(MessageContext context, Message message, Data data)
        {
            Console.WriteLine($"SagaInputTopicHandlerMessageHandler Name {message.Name}");
            data.Status = "SagaInputTopicHandlerMessageHandler";
            _bus.Publish(message, new Options() {EndPointName = "SagaInputTopic2HandlerMessageHandler", SagaContext = context.SagaContext });
        }
    }

    public class SagaInputTopic2HandlerMessageHandler : IMessageSagaHandler<Message>
    {
        private readonly IBus _bus;

        public SagaInputTopic2HandlerMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(MessageContext context, Message message, Data data)
        {
            Console.WriteLine($"SagaInputTopic2HandlerMessageHandler Name {message.Name}");
            data.Status = "SagaInputTopic2HandlerMessageHandler";

        }
    }
}