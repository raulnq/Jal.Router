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
        public static MessageContext CreateMessageContext(Route route=null, EndPoint endpoint=null,  Mock<IBus> busmock = null, IMessageSerializer serializer = null)
        {
            if(busmock==null)
            {
                busmock = new Mock<IBus>();
            }

            if (route == null)
            {
                route = new Route("route", typeof(ConsumerMiddleware));
            }


            if (endpoint == null)
            {
                endpoint = new EndPoint("endpoint");
            }

            if(serializer==null)
            {
                serializer = new NullMessageSerializer();
            }

            var messagecontext = new MessageContext(busmock.Object, serializer, Guid.NewGuid().ToString(), route, CreateChannel(), endpoint);

            return messagecontext;
        }

        public static MessageContext CreateMessageContextWithSaga(Route route = null, string status=null)
        {
            if(route == null)
            {
                route = new Route(new Saga("name", typeof(object)), "name", typeof(object));
            }

            var messagecontext = CreateMessageContext(route);

            messagecontext.SagaContext.Load(new SagaData(new object(), typeof(object), "name", DateTime.Now, 0, status));

            //var sagacontext = new SagaContext(messagecontext, "id");

            //sagacontext.Load(new SagaData(new object(), typeof(object), "name", DateTime.Now, 0, status));

            //messagecontext.UpdateSagaContext(sagacontext);

            return messagecontext;
        }

        public static Route CreateRoute()
        {
            var route = new Route("route", typeof(ConsumerMiddleware));

            return route;
        }

        public static Route CreateRouteWithConsumer(string status, Func<MessageContext, bool> condition = null)
        {
            var route = CreateRoute();

            var method = new RouteMethodWithData<object, Handler, object>((o,h,m,d)=>Task.CompletedTask, typeof(Handler), typeof(object), condition, status);

            route.RouteMethods.Add(method);

            return route;
        }

        public static Route CreateRouteWithConsumer(Func<MessageContext, bool> condition = null)
        {
            var route = CreateRoute();

            var method = new RouteMethod<object, Handler>((o, h, m) => Task.CompletedTask, typeof(Handler), typeof(object), condition);

            route.RouteMethods.Add(method);

            return route;
        }

        public static Route CreateRouteWithConsumer(Func<object, Handler, MessageContext, Task> consumer,Func<MessageContext, bool> condition= null)
        {
            var route = CreateRoute();

            var method = new RouteMethod<object, Handler>(consumer, typeof(Handler), typeof(object), condition);

            route.RouteMethods.Add(method);

            return route;
        }

        public static Route CreateRouteWithConsumer(Func<object, Handler, object, Task> consumer, Func<MessageContext, bool> condition = null)
        {
            var route = CreateRoute();

            var method = new RouteMethod<object, Handler>(consumer, typeof(Handler), typeof(object), condition);

            route.RouteMethods.Add(method);

            return route;
        }

        public static Route CreateRouteWithConsumer(Func<object, Handler, MessageContext, object, Task> consumer, Func<MessageContext, bool> condition=null)
        {
            var route = CreateRoute();

            var method = new RouteMethodWithData<object, Handler, object>(consumer, typeof(Handler), typeof(object), condition);

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

            endpoint.Channels.Add(new Channel(channel, null, null, null, null));

            endpointprovidermock.Setup(x => x.Provide(It.IsAny<Options>(), It.IsAny<Type>())).Returns(endpoint);

            return endpointprovidermock;
        }

        public static Channel CreateChannel(ChannelType channeltype = ChannelType.PointToPoint, string connectionstring = "connectionstring", string path = "path", string subscription = "subscription", Type adapter = null, Type type = null)
        {
            return new Channel(channeltype, connectionstring, path, subscription, adapter, type);
        }

        public static SenderContext CreateSenderContext(EndPoint endpoint = null, Channel channel = null, ISenderChannel senderchannel=null, IReaderChannel readerchannel=null, IMessageAdapter adapter = null, IMessageSerializer serializer = null, IMessageStorage storage = null)
        {
            if(channel==null)
            {
                channel = CreateChannel();
            }

            if (endpoint == null)
            {
                endpoint = new EndPoint("name");
            }

            return new SenderContext(endpoint, channel, senderchannel, readerchannel, adapter, serializer, storage);
        }

        public static ListenerContext CreateListenerContext(Route route = null, Channel channel = null, IListenerChannel listenerchannel = null,  IMessageAdapter adapter=null, IRouter router=null, IMessageSerializer serializer = null, IMessageStorage storage=null)
        {
            if (channel == null)
            {
                channel = CreateChannel();
            }

            if (route == null)
            {
                route = CreateRoute();
            }

            return new ListenerContext(route, channel, listenerchannel, adapter, router, serializer, storage);
        }

        public static Mock<IComponentFactoryFacade> CreateFactoryMock()
        {
            var factorymock = new Mock<IComponentFactoryFacade>();

            factorymock.Setup(m => m.CreateRouterInterceptor()).Returns(new NullRouterInterceptor());

            factorymock.Setup(m => m.CreateBusInterceptor()).Returns(new NullBusInterceptor());

            factorymock.Setup(x => x.CreateMessageSerializer()).Returns(new NullMessageSerializer());

            factorymock.Setup(x => x.CreateMessageAdapter(It.IsAny<Type>())).Returns(new NullMessageAdapter());

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

        public static Mock<IComponentFactoryFacade> CreateFactoryMockWithHandler<T>() where T : class, new() 
        {
            var factorymock = CreateFactoryMock();

            factorymock.Setup(x => x.CreateComponent<T>(typeof(T))).Returns(new T());

            return factorymock;
        }
    }
}
