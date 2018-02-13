using System;
using System.Threading;
using Jal.Router.Extensions;
using Jal.Router.Impl;
using Jal.Router.Interface.Inbound.Sagas;
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

    public class TriggerFlowAHandler : IRequestResponseHandler<Trigger>
    {
        public void Handle(Trigger message, MessageContext context)
        {
            context.Send(new RequestToSend() { Name = "Hello world!!" }, "appa");
        }
    }

    public class RequestToSendAppAHandler : IRequestResponseHandler<RequestToSend>
    {
        public void Handle(RequestToSend message, MessageContext context)
        {
            context.Send(new ResponseToSend() { Name = message.Name }, "appb");
        }
    }

    public class ResponseToSendAppBHandler : IRequestResponseHandler<ResponseToSend>
    {
        public void Handle(ResponseToSend message, MessageContext context)
        {
            Console.WriteLine(message.Name);
        }
    }

    public class TriggerFlowBHandler : IRequestResponseHandler<Trigger>
    {
        public void Handle(Trigger message, MessageContext context)
        {
            context.Send(new RequestToSend() { Name = "Hello world!!" }, "appc");
        }
    }

    public class RequestToSendAppCHandler : IRequestResponseHandler<RequestToSend>
    {
        public void Handle(RequestToSend message, MessageContext context)
        {
            context.Publish(new ResponseToSend() { Name = message.Name }, "appd");
        }
    }

    public class ResponseToSendAppDHandler : IRequestResponseHandler<ResponseToSend>
    {
        public void Handle(ResponseToSend message, MessageContext context)
        {
            Console.WriteLine(message.Name);
        }
    }

    public class RequestToSendAppEHandler : IRequestResponseHandler<RequestToSend, Data>
    {
        public void Handle(RequestToSend message, MessageContext context, Data data)
        {
            data.Status = "Start";

            context.SendWithSagaInfo(data, new ResponseToSend() { Name = message.Name }, "appx", $"{context.Id}@child-1");
            
        }
    }

    public class RequestToSendAppXHandler : IRequestResponseHandler<ResponseToSend, Data>
    {
        public void Handle(ResponseToSend message, MessageContext context, Data data)
        {
            context.SendWithParentSagaInfo(new ResponseToSend() { Name = message.Name }, "appf");
        }
    }

        public class RequestToSendAppZHandler : IRequestResponseHandler<ResponseToSend>
    {

        public void Handle(ResponseToSend message, MessageContext context)
        {
            context.SendWithSagaInfo(new ResponseToSend() { Name = message.Name }, "apph");
            
        }
    }

    public class ResponseToSendAppFHandler : IRequestResponseHandler<ResponseToSend, Data>
    {
        public void Handle(ResponseToSend message, MessageContext context, Data data)
        {
            Console.WriteLine(message.Name + " " + data.Status);
            data.Status = "Continue";

            context.SendWithSagaInfo(data, new ResponseToSend() { Name = message.Name }, "appz", $"{context.Id}@child-2");
        }
    }

    public class ResponseToSendAppHHandler : IRequestResponseHandler<ResponseToSend, Data>
    {
        public void Handle(ResponseToSend message, MessageContext contextM, Data data)
        {
            
            Console.WriteLine(message.Name + " " + data.Status);
            data.Status = "end";
        }
    }

    public class TriggerFlowCHandler : IRequestResponseHandler<Trigger>
    {
        public void Handle(Trigger message, MessageContext context)
        {
            context.Send<RequestToSend>(new RequestToSend() { Name = "Hello world!!" }, new Origin() {Key = "system"}, "appe", "parent");
        }
    }

    public class TriggerFlowDHandler : IRequestResponseHandler<Trigger>
    {
        public void Handle(Trigger message, MessageContext context)
        {
            context.Send(new RequestToSend() { Name = "Hello world!!" }, "appg");
        }
    }

    public class TriggerFlowEHandler : IRequestResponseHandler<Trigger>
    {
        private readonly IBus _bus;

        public TriggerFlowEHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(Trigger message, MessageContext context)
        {
            context.Send(new RequestToSend() { Name = "Hello world!!" }, "appi");
        }
    }


    public class ResponseToSendAppGHandler : IRequestResponseHandler<RequestToSend>
    {
        public void Handle(RequestToSend message, MessageContext context)
        {
            Console.WriteLine(message.Name);
        }
    }

    public class ResponseToSendAppIHandler : IRequestResponseHandler<RequestToSend>
    {
        public void Handle(RequestToSend message, MessageContext context)
        {
            Console.WriteLine(message.Name + " I");
        }
    }

    public class ResponseToSendAppJHandler : IRequestResponseHandler<RequestToSend>
    {
        public void Handle(RequestToSend message, MessageContext context)
        {
            Console.WriteLine(message.Name + " J");
        }
    }

    public class RequestHandler : IRequestResponseHandler<RequestToSend>
    {
        public void Handle(RequestToSend message, MessageContext context)
        {
            Console.WriteLine($"request {message.Name}");

            context.Publish(new ResponseToSend() { Name = message.Name }, new Options() { RequestId = context.ReplyToRequestId, EndPointName = "toresponsetopic" });
        }
    }

    public interface IRequestResponseHandler<in T>
    {
        void Handle(T message, MessageContext context);
    }

    public interface IRequestResponseHandler<in T, in D>
    {
        void Handle(T message, MessageContext context, D data);
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