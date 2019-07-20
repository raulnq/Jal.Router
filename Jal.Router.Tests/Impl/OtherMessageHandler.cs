using System;
using System.Threading.Tasks;
using Jal.Router.Extensions;
using Jal.Router.Interface;
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

        public Task Handle(Message message, Data data)
        {
            Console.WriteLine($"Other Sender {message.Name}");

            //return _bus.Send(message, new Options() {EndPointName = "route1" });

            //throw new ApplicationException();

            return Task.CompletedTask;
        }
    }

    public class SagaInput1HandlerMessageHandler : IMessageSagaHandler<Message>
    {
        private readonly IBus _bus;

        public SagaInput1HandlerMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public Task Handle(MessageContext context, Message message, Data data)
        {
            Console.WriteLine($"SagaInput1HandlerMessageHandler Name {message.Name}");
            data.Status = "SagaInput1HandlerMessageHandler";
            //throw new Exception("error");
            return context.Publish(message, "SagaInputTopicHandlerMessageHandler");
        }
    }

    public class SagaInputTopicHandlerMessageHandler  : IMessageSagaHandler<Message>
    {
        private readonly IBus _bus;

        public SagaInputTopicHandlerMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public Task Handle(MessageContext context, Message message, Data data)
        {
            Console.WriteLine($"SagaInputTopicHandlerMessageHandler Name {message.Name}");
            data.Status = "SagaInputTopicHandlerMessageHandler";
            return context.Publish(message, "SagaInputTopic2HandlerMessageHandler");
        }
    }

    public class SagaInputTopic2HandlerMessageHandler : IMessageSagaHandler<Message>
    {
        private readonly IBus _bus;

        public SagaInputTopic2HandlerMessageHandler(IBus bus)
        {
            _bus = bus;
        }

        public Task Handle(MessageContext context, Message message, Data data)
        {
            Console.WriteLine($"SagaInputTopic2HandlerMessageHandler Name {message.Name}");
            data.Status = "SagaInputTopic2HandlerMessageHandler";

            return Task.CompletedTask;

        }
    }
}