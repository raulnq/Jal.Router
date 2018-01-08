using System;
using Jal.Router.Impl;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class MessageHandler : IMessageHandler<Message>
    {
        public void Handle(Message message, Data response)
        {
            Console.WriteLine("Sender");
            response.Status = "Start";
        }

    }

    public class Message1Handler : IMessageHandler<Message1>
    {
        public void Handle(Message1 message, Data response)
        {
            Console.WriteLine("Sender1");
            //response.Status = "End";
        }

    }

    public class TriggerHandler : IRequestResponseHandler<Trigger>
    {
        private readonly IBus _bus;

        public TriggerHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(Trigger message, MessageContext context)
        {
            var result = _bus.Reply<RequestToSend, ResponseToSend>(new RequestToSend() {Name = "Hi Raul"}, new Options() {ReplyToRequestId = Guid.NewGuid().ToString(), EndPointName = "torequestqueue" } );

            if (result == null)
            {
                Console.WriteLine("No response");
            }
            else
            {
                Console.WriteLine($"trigger {result.Name}");
            }
            
        }
    }

    public class RequestHandler : IRequestResponseHandler<RequestToSend>
    {
        private readonly IBus _bus;

        public RequestHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(RequestToSend message, MessageContext context)
        {
            Console.WriteLine($"request {message.Name}");
            _bus.Publish(new ResponseToSend() {Name = message.Name}, new Options() {RequestId = context.ReplyToRequestId, EndPointName = "toresponsetopic" } );
        }
    }

    public interface IRequestResponseHandler<in T>
    {
        void Handle(T message, MessageContext context);
    }

    public interface IMessageHandler<in T>
    {
        void Handle(T message, Data response);
    }

    public interface IMessageSagaHandler<in T>
    {
        void Handle(MessageContext context, T message, Data response);
    }
}