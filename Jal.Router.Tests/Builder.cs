using Jal.ChainOfResponsability;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    public static class Builder
    {
        public static MessageContext CreateMessageContext(Route route=null, EndPoint endpoint=null,  Mock<IBus> busmock = null)
        {
            if(busmock==null)
            {
                busmock = new Mock<IBus>();
            }

            var messagecontext = new MessageContext(busmock.Object);

            if(route==null)
            {
                messagecontext.SetRoute(new Route("route", typeof(object), new List<Channel>()));
            }
            else
            {
                messagecontext.SetRoute(route);
            }

            if (endpoint == null)
            {
                messagecontext.SetEndPoint(new EndPoint("endpoint"));
            }
            else
            {
                messagecontext.SetEndPoint(endpoint);
            }

            return messagecontext;
        }

        public static MessageContext CreateMessageContextWithSaga(Route route = null, string status=null)
        {
            var messagecontext = CreateMessageContext(route);

            messagecontext.SetSaga(new Saga("name", typeof(object)));

            var sagacontext = new SagaContext(messagecontext, "id");

            sagacontext.Load(new SagaData(new object(), typeof(object), "name", DateTime.Now, 0, status));

            messagecontext.UpdateSagaContext(sagacontext);

            return messagecontext;
        }

        public static Route CreateRoute()
        {
            var route = new Route("route", typeof(object), new List<Channel>() { });

            return route;
        }

        public static Route CreateRouteWithConsumer(string status)
        {
            var route = CreateRoute();

            var method = new RouteMethodWithData<object, Handler, object>((o,h,m,d)=>Task.CompletedTask, typeof(Handler), status);

            route.RouteMethods.Add(method);

            return route;
        }

        public static Route CreateRouteWithConsumer()
        {
            var route = CreateRoute();

            var method = new RouteMethod<object, Handler>((o, h, m) => Task.CompletedTask, typeof(Handler));

            route.RouteMethods.Add(method);

            return route;
        }

        public static Route CreateRouteWithConsumer(Func<object, Handler, MessageContext, Task> consumer)
        {
            var route = CreateRoute();

            var method = new RouteMethod<object, Handler>(consumer, typeof(Handler));

            route.RouteMethods.Add(method);

            return route;
        }

        public static Route CreateRouteWithConsumer(Func<object, Handler, object, Task> consumer)
        {
            var route = CreateRoute();

            var method = new RouteMethod<object, Handler>(consumer, typeof(Handler));

            route.RouteMethods.Add(method);

            return route;
        }

        public static Route CreateRouteWithConsumer(Func<object, Handler, MessageContext, object, Task> consumer)
        {
            var route = CreateRoute();

            var method = new RouteMethodWithData<object, Handler, object>(consumer, typeof(Handler));

            route.RouteMethods.Add(method);

            return route;
        }

        public static Mock<IPipeline> CreatePipelineMock(bool throwexception = false)
        {
            var pipelinemock = new Mock<IPipeline>();

            if (throwexception)
            {
                pipelinemock.Setup(x => x.ExecuteAsync(It.IsAny<AsyncMiddlewareConfiguration<MessageContext>[]>(), It.IsAny<MessageContext>(), It.IsAny<CancellationToken>())).Throws(new System.Exception());
            }
            else
            {
                pipelinemock.Setup(x => x.ExecuteAsync(It.IsAny<AsyncMiddlewareConfiguration<MessageContext>[]>(), It.IsAny<MessageContext>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            }

            return pipelinemock;
        }

        public static Mock<IEndPointProvider> CreateEnpointProvider(string endpointname = "endpointname", ChannelType channel= ChannelType.PointToPoint)
        {
            var endpointprovidermock = new Mock<IEndPointProvider>();

            var endpoint = new EndPoint(endpointname);

            endpoint.SetOrigin(new Origin());

            endpoint.Channels.Add(new Channel(channel, null, null, null));

            endpointprovidermock.Setup(x => x.Provide(It.IsAny<string>(), It.IsAny<Type>())).Returns(endpoint);

            return endpointprovidermock;
        }

        public static Channel CreateChannel(ChannelType channeltype = ChannelType.PointToPoint, string connectionstring = "connectionstring", string path = "path", string subscription = "subscription")
        {
            return new Channel(channeltype, connectionstring, path, subscription);
        }

        public static SenderContext CreateSenderContext(Channel channel = null, ISenderChannel senderchannel=null, IReaderChannel readerchannel=null )
        {
            if(channel==null)
            {
                channel = CreateChannel();
            }

            return new SenderContext(channel, senderchannel, readerchannel);
        }

        public static ListenerContext CreateListenerContext(Channel channel = null, IListenerChannel listenerchannel = null, Partition partition= null)
        {
            if (channel == null)
            {
                channel = CreateChannel();
            }

            return new ListenerContext(channel, listenerchannel, partition);
        }

        public static Mock<IComponentFactoryGateway> CreateFactoryMock()
        {
            var factorymock = new Mock<IComponentFactoryGateway>();

            factorymock.Setup(m => m.CreateRouterInterceptor()).Returns(new NullRouterInterceptor());

            factorymock.Setup(m => m.CreateBusInterceptor()).Returns(new NullBusInterceptor());

            factorymock.Setup(x => x.CreateMessageSerializer()).Returns(new NullMessageSerializer());

            factorymock.Setup(x => x.CreateMessageAdapter()).Returns(new NullMessageAdapter());

            factorymock.Setup(m => m.Configuration).Returns(new Configuration());

            return factorymock;
        }

        public static Mock<IEntityStorage> CreateEntityStorage(string id=null)
        {
            var entitystoragemock = new Mock<IEntityStorage>();

            entitystoragemock.Setup(x => x.Create(It.IsAny<SagaData>())).ReturnsAsync(id);

            entitystoragemock.Setup(x => x.Update(It.IsAny<SagaData>()));

            entitystoragemock.Setup(x => x.Create(It.IsAny<MessageEntity>())).ReturnsAsync(id);

            entitystoragemock.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(new SagaData(id, new object(), typeof(object), "name", DateTime.Now, 0, string.Empty, null, null, 0));

            return entitystoragemock;
        }

        public static Mock<IComponentFactoryGateway> CreateFactoryMockWithHandler<T>() where T : class, new() 
        {
            var factorymock = CreateFactoryMock();

            factorymock.Setup(x => x.CreateComponent<T>(typeof(T))).Returns(new T());

            return factorymock;
        }
    }
}
