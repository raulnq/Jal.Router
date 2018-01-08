using System;
using System.Threading;
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
        private readonly IBus _bus;

        public TriggerFlowAHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(Trigger message, MessageContext context)
        {
            _bus.Send(new RequestToSend() { Name = "Hello world!!" }, new Options() {EndPointName = "appa" });
        }
    }

    public class RequestToSendAppAHandler : IRequestResponseHandler<RequestToSend>
    {
        private readonly IBus _bus;

        public RequestToSendAppAHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(RequestToSend message, MessageContext context)
        {
            _bus.Send(new ResponseToSend() { Name = message.Name }, new Options() {EndPointName = "appb" });
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
        private readonly IBus _bus;

        public TriggerFlowBHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(Trigger message, MessageContext context)
        {
            _bus.Send(new RequestToSend() { Name = "Hello world!!" }, new Options() { EndPointName = "appc" });
        }
    }

    public class RequestToSendAppCHandler : IRequestResponseHandler<RequestToSend>
    {
        private readonly IBus _bus;

        public RequestToSendAppCHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(RequestToSend message, MessageContext context)
        {
            _bus.Publish(new ResponseToSend() { Name = message.Name }, new Options() { EndPointName = "appd"});
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
        private readonly IBus _bus;

        private readonly IStorageFacade _storage;

        public RequestToSendAppEHandler(IBus bus, IStorageFacade storage)
        {
            _bus = bus;
            _storage = storage;
        }

        public void Handle(RequestToSend message, MessageContext context, Data data)
        {
            data.Status = "Complete";
            _bus.Send(new ResponseToSend() { Name = message.Name }, new Options() { EndPointName = "appx", SagaInfo = context.SagaInfo});
            
        }
    }

    public class RequestToSendAppXHandler : IRequestResponseHandler<ResponseToSend>
    {
        private readonly IBus _bus;

        public RequestToSendAppXHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(ResponseToSend message, MessageContext context)
        {
            Thread.Sleep(10000);
            _bus.Send(new ResponseToSend() { Name = message.Name }, new Options() { EndPointName = "appf", SagaInfo = context.SagaInfo });
        }
    }

    public class ResponseToSendAppFHandler : IRequestResponseHandler<ResponseToSend, Data>
    {
        public void Handle(ResponseToSend message, MessageContext contextM, Data data)
        {
            Console.WriteLine(message.Name+ " " + data.Status);
        }
    }

    public class TriggerFlowCHandler : IRequestResponseHandler<Trigger>
    {
        private readonly IBus _bus;

        public TriggerFlowCHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(Trigger message, MessageContext context)
        {
            _bus.Send(new RequestToSend() { Name = "Hello world!!" }, new Options() { EndPointName = "appe" });
        }
    }

    public class TriggerFlowDHandler : IRequestResponseHandler<Trigger>
    {
        private readonly IBus _bus;

        public TriggerFlowDHandler(IBus bus)
        {
            _bus = bus;
        }

        public void Handle(Trigger message, MessageContext context)
        {
            _bus.Send(new RequestToSend() { Name = "Hello world!!" }, new Options() { EndPointName = "appg" });
        }
    }


    public class ResponseToSendAppGHandler : IRequestResponseHandler<RequestToSend>
    {
        public void Handle(RequestToSend message, MessageContext context)
        {
            Console.WriteLine(message.Name + " " + context.RetryCount.ToString());
            throw new ApplicationException("Error");
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