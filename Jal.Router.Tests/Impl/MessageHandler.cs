using System;
using System.Threading.Tasks;
using Jal.Router.Extensions;
using Jal.Router.Interface;
using Jal.Router.Model;
using Jal.Router.Tests.Model;

namespace Jal.Router.Tests.Impl
{
    public class MessageHandler : IMessageHandler<Message>
    {
        public Task Handle(Message message, Data response)
        {
            Console.WriteLine("Sender");

            response.Status = "Start";

            return Task.CompletedTask;
        }

    }

    public class Message1Handler : IMessageHandler<Message1>
    {
        public Task Handle(Message1 message, Data response)
        {
            Console.WriteLine("Sender1");
            //response.Status = "End";

            return Task.CompletedTask;
        }

    }

    public class TriggerHandler : IRequestResponseHandler<Trigger>
    {
        private readonly IBus _bus;

        public TriggerHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(Trigger message, MessageContext context)
        {

            var result = await context.Reply<RequestToSend, ResponseToSend>(new RequestToSend() {Name = "Hi Raul"}, "torequestqueue");

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
        public Task Handle(Trigger message, MessageContext context)
        {
            return context.Send(new RequestToSend() { Name = "Hello world!!" }, "appa", context.IdentityContext);
        }
    }

    public class RequestToSendAppAHandler : IRequestResponseHandler<RequestToSend>
    {
        public Task Handle(RequestToSend message, MessageContext context)
        {
            return context.Send(new ResponseToSend() { Name = message.Name }, "appb", context.IdentityContext);
        }
    }

    public class ResponseToSendAppBHandler : IRequestResponseHandler<ResponseToSend>
    {
        public Task Handle(ResponseToSend message, MessageContext context)
        {
            Console.WriteLine(message.Name);

            return Task.CompletedTask;
        }
    }

    public class TriggerFlowBHandler : IRequestResponseHandler<Trigger>
    {
        public Task Handle(Trigger message, MessageContext context)
        {
            return context.Send(new RequestToSend() { Name = "Hello world!!" }, "appc", context.IdentityContext);
        }
    }

    public class RequestToSendAppCHandler : IRequestResponseHandler<RequestToSend>
    {
        public Task Handle(RequestToSend message, MessageContext context)
        {
            return context.Publish(new ResponseToSend() { Name = message.Name }, "appd", context.IdentityContext, context.Origin.Key);
        }
    }

    public class ResponseToSendAppDHandler : IRequestResponseHandler<ResponseToSend>
    {
        public Task Handle(ResponseToSend message, MessageContext context)
        {
            Console.WriteLine(message.Name);

            return Task.CompletedTask;
        }
    }

    public class RequestToSendAppEHandler : IRequestResponseHandler<RequestToSend, Data>
    {
        public Task Handle(RequestToSend message, MessageContext context, Data data)
        {
            data.Status = "Start";

            var identity = new IdentityContext($"{context.IdentityContext.Id}@child-1", context.IdentityContext.Id);

            return context.Send(new ResponseToSend() { Name = message.Name }, "appx", identity, context.SagaContext.Id);
        }
    }

    public class RequestToSendAppXHandler : IRequestResponseHandler<ResponseToSend, Data>
    {
        public Task Handle(ResponseToSend message, MessageContext context, Data data)
        {
            var caller = context.TrackingContext.GetTrackOfTheSagaCaller();

            return context.Send(new ResponseToSend() { Name = message.Name }, "appf", caller.Id, caller.SagaId);
        }
    }

        public class RequestToSendAppZHandler : IRequestResponseHandler<ResponseToSend>
    {

        public Task Handle(ResponseToSend message, MessageContext context)
        {
            return context.Send(new ResponseToSend() { Name = message.Name }, "apph", context.SagaContext.Id);
        }
    }

    public class ResponseToSendAppFHandler : IRequestResponseHandler<ResponseToSend, Data>
    {
        public Task Handle(ResponseToSend message, MessageContext context, Data data)
        {
            Console.WriteLine(message.Name + " " + data.Status);
            data.Status = "Continue";

            var identity = new IdentityContext($"{context.IdentityContext.Id}@child-2", context.IdentityContext.Id);

            return context.Send(new ResponseToSend() { Name = message.Name }, "appz", identity, context.SagaContext.Id);
        }
    }

    public class ResponseToSendAppHHandler : IRequestResponseHandler<ResponseToSend, Data>
    {
        public Task Handle(ResponseToSend message, MessageContext contextM, Data data)
        {
            
            Console.WriteLine(message.Name + " " + data.Status);

            data.Status = "end";

            return Task.CompletedTask;
        }
    }

    public class TriggerFlowCHandler : IRequestResponseHandler<Trigger>
    {
        public Task Handle(Trigger message, MessageContext context)
        {
            return context.Send<RequestToSend>(new RequestToSend() { Name = "Hello world!!" }, "appe", "parentid");
        }
    }

    public class TriggerFlowDHandler : IRequestResponseHandler<Trigger>
    {
        public Task Handle(Trigger message, MessageContext context)
        {
            return context.Send(new RequestToSend() { Name = "Hello world!!" }, "appg", context.IdentityContext);
        }
    }

    public class TriggerFlowEHandler : IRequestResponseHandler<Trigger>
    {
        private readonly IBus _bus;

        public TriggerFlowEHandler(IBus bus)
        {
            _bus = bus;
        }

        public Task Handle(Trigger message, MessageContext context)
        {
            return context.Send(new RequestToSend() { Name = "Hello world!!" }, "appi", context.IdentityContext);

        }
    }


    public class ResponseToSendAppGHandler : IRequestResponseHandler<RequestToSend>
    {
        public Task Handle(RequestToSend message, MessageContext context)
        {
            Console.WriteLine(message.Name);

            return Task.CompletedTask;
        }
    }

    public class ResponseToSendAppIHandler : IRequestResponseHandler<RequestToSend>
    {
        public Task Handle(RequestToSend message, MessageContext context)
        {
            Console.WriteLine(message.Name + " I");

            return Task.CompletedTask;
        }
    }

    public class ResponseToSendAppJHandler : IRequestResponseHandler<RequestToSend>
    {
        public Task Handle(RequestToSend message, MessageContext context)
        {
            Console.WriteLine(message.Name + " J");

            return Task.CompletedTask;
        }
    }

    public class RequestHandler : IRequestResponseHandler<RequestToSend>
    {
        public Task Handle(RequestToSend message, MessageContext context)
        {
            Console.WriteLine($"request {message.Name}");

            return context.Publish(new ResponseToSend() { Name = message.Name }, "toresponsetopic");
        }
    }

    public interface IRequestResponseHandler<in T>
    {
        Task Handle(T message, MessageContext context);
    }

    public interface IRequestResponseHandler<in T, in D>
    {
        Task Handle(T message, MessageContext context, D data);
    }

    public interface IMessageHandler<in T>
    {
        Task Handle(T message, Data response);
    }

    public interface IMessageSagaHandler<in T>
    {
        Task Handle(MessageContext context, T message, Data response);
    }
}