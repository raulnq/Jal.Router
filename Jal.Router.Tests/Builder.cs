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
        public static EndPoint CreateEndpoint(string name = "endpoint", bool when = true)
        {
            var endpoint = new EndPoint(name, typeof(object));

            endpoint.When((e, o, t) => when);

            return endpoint;
        }

        public static MessageContext CreateMessageContextToSend(IComponentFactoryFacade factory=null, EndPoint endpoint = null, Channel channel= null)
        {
            if(factory==null)
            {
                factory = CreateFactoryMock().Object;
            }

            if (endpoint == null)
            {
                endpoint = CreateEndpoint();
            }

            if (channel == null)
            {
                channel = CreateChannel();
            }

            var options = Options.CreateEmpty(endpoint.Name);

            return MessageContext.CreateToSend(factory.CreateMessageSerializer(), factory.CreateEntityStorage(), endpoint, channel, options, new Origin(), null, DateTime.UtcNow);
        }

        public static MessageContext CreateMessageContextFromListen(IComponentFactoryFacade factory = null, Route route=null, Channel channel = null, IBus bus = null)
        {
            if(bus == null)
            {
                bus = new Mock<IBus>().Object;
            }

            if (route == null)
            {
                route = new Route("route", typeof(ConsumerMiddleware));
            }

            if (channel == null)
            {
                channel = CreateChannel();
            }

            if (factory == null)
            {
                factory = CreateFactoryMock().Object;
            }

            var messagecontext = MessageContext.CreateFromListen(bus, factory.CreateMessageSerializer(), factory.CreateEntityStorage(), route, null,
                channel, new TracingContext(Guid.NewGuid().ToString()), new List<Tracking>(), new Origin(), 
                string.Empty, string.Empty, string.Empty, DateTime.UtcNow, string.Empty);

            return messagecontext;
        }

        public static Route CreateRoute()
        {
            var route = new Route("route", typeof(ConsumerMiddleware));

            return route;
        }

        public static Route CreateRouteWithSaga()
        {
            var route = new Route(new Saga("name", typeof(object)), "route", typeof(ConsumerMiddleware));

            return route;
        }

        public static Route CreateRouteWithSagaAndConsumer(string status="", Func<MessageContext, bool> condition = null)
        {
            var route = CreateRouteWithSaga();

            var method = new RouteMethodWithData<object, Handler, object>((o, h, m, d) => Task.CompletedTask, typeof(Handler), typeof(object), condition, status);

            route.RouteMethods.Add(method);

            return route;
        }

        public static SagaData CreateSagaData(string status="", bool empty=false)
        {
            if(empty)
            {
                return new SagaData(null, typeof(object), "name", DateTime.Now, 0, status);
            }

            return new SagaData(new object(), typeof(object), "name", DateTime.Now, 0, status);
        }

        public static Route CreateRouteWithConsumer(Func<MessageContext, bool> condition = null)
        {
            var route = CreateRoute();

            var method = new RouteMethod<object, Handler>((o, h, m) => Task.CompletedTask, typeof(Handler), typeof(object), condition);

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

        public static Mock<IEndPointProvider> CreateEnpointProviderMock(string endpointname = "endpointname", ChannelType channel= ChannelType.PointToPoint)
        {
            var endpointprovidermock = new Mock<IEndPointProvider>();

            var endpoint = CreateEndpoint();

            endpoint.SetOrigin(new Origin());

            endpoint.Channels.Add(new Channel(channel, null, null, null, null));

            endpointprovidermock.Setup(x => x.Provide(It.IsAny<Options>(), It.IsAny<Type>())).Returns(endpoint);

            return endpointprovidermock;
        }

        public static Channel CreateChannel(ChannelType channeltype = ChannelType.PointToPoint, string connectionstring = "connectionstring", string path = "path", string subscription = "subscription", Type adapter = null, Type type = null)
        {
            return new Channel(channeltype, connectionstring, path, subscription, adapter, type);
        }

        public static SenderContext CreateSenderContext(IComponentFactoryFacade factory = null, EndPoint endpoint = null, Channel channel = null)
        {
            if(channel==null)
            {
                channel = CreateChannel();
            }

            if (endpoint == null)
            {
                endpoint = CreateEndpoint();
            }

            if (factory == null)
            {
                factory = CreateFactoryMock().Object;
            }

            return SenderContext.Create(factory, new NullLogger(), channel, endpoint);
        }

        public static ListenerContext CreateListenerContext(IComponentFactoryFacade factory = null, IRouter router = null, Route route = null, Channel channel = null)
        {
            if (channel == null)
            {
                channel = CreateChannel();
            }

            if (route == null)
            {
                route = CreateRoute();
            }

            if(factory==null)
            {
                factory = CreateFactoryMock().Object;
            }

            return ListenerContext.Create(factory, router, new NullLogger(), channel, route);
        }

        public static Mock<IComponentFactoryFacade> CreateFactoryMock()
        {
            var factorymock = new Mock<IComponentFactoryFacade>();

            factorymock.Setup(m => m.CreateRouterInterceptor()).Returns(new NullRouterInterceptor());

            factorymock.Setup(m => m.CreateBusInterceptor()).Returns(new NullBusInterceptor());

            factorymock.Setup(x => x.CreateMessageSerializer()).Returns(new NullMessageSerializer());

            factorymock.Setup(x => x.CreateEntityStorage()).Returns(new NullEntityStorage());

            factorymock.Setup(x => x.CreateMessageStorage()).Returns(new NullMessageStorage());

            factorymock.Setup(x => x.CreateMessageAdapter(It.IsAny<Type>())).Returns(new NullMessageAdapter());

            factorymock.Setup(m => m.CreateListenerChannel(It.IsAny<ChannelType>(), It.IsAny<Type>())).Returns(new NullPointToPointChannel());

            factorymock.Setup(m => m.CreateSenderChannel(It.IsAny<ChannelType>(), It.IsAny<Type>())).Returns((new NullPointToPointChannel(), null));

            factorymock.Setup(m => m.Configuration).Returns(new Configuration());

            return factorymock;
        }

        public static Mock<ISenderContextLifecycle> CreateSenderContextLifecycleMock(IComponentFactoryFacade factory = null, Channel channel = null, EndPoint endpoint = null)
        {
            var lifecyclemock = new Mock<ISenderContextLifecycle>();

            lifecyclemock.Setup(x => x.Get(It.IsAny<Channel>())).Returns(Builder.CreateSenderContext(factory, endpoint, channel));

            return lifecyclemock;
        }
        public static Mock<IEntityStorage> CreateEntityStorageMock(string id=null)
        {
            var entitystoragemock = new Mock<IEntityStorage>();

            entitystoragemock.Setup(x => x.Insert(It.IsAny<SagaData>(), It.IsAny<IMessageSerializer>())).ReturnsAsync(id);

            entitystoragemock.Setup(x => x.Update(It.IsAny<SagaData>(), It.IsAny<IMessageSerializer>()));

            entitystoragemock.Setup(x => x.Insert(It.IsAny<MessageEntity>(), It.IsAny<IMessageSerializer>())).ReturnsAsync(id);

            entitystoragemock.Setup(x => x.Get(It.IsAny<string>(), It.IsAny<IMessageSerializer>())).ReturnsAsync(new SagaData(id, new object(), typeof(object), "name", DateTime.Now, 0, string.Empty, null, null, 0));

            return entitystoragemock;
        }

        public static void AddCreateComponent<T>(this Mock<IComponentFactoryFacade> factorymock) where T : class, new() 
        {
            factorymock.Setup(x => x.CreateComponent<T>(typeof(T))).Returns(new T());
        }

        public static void AddEntityStorage(this Mock<IComponentFactoryFacade> factorymock, Mock<IEntityStorage> entitystoragemock)
        {
            factorymock.Setup(x => x.CreateEntityStorage()).Returns(entitystoragemock.Object);
        }

        public static void AddChannelShuffler(this Mock<IComponentFactoryFacade> factorymock, Channel channel)
        {
            var shuffermock = new Mock<IChannelShuffler>();

            shuffermock.Setup(x => x.Shuffle(It.IsAny<Channel[]>())).Returns(new Channel[] { channel });

            factorymock.Setup(m => m.CreateChannelShuffler()).Returns(shuffermock.Object);
        }

        public static Mock<IConsumer> CreateConsumerMock()
        {
            return new Mock<IConsumer>();
        }
    }
}
