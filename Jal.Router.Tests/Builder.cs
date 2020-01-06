﻿using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jal.Router.Tests
{
    public static class Builder
    {
        public static MessageContext CreateMessageContext(Route route=null)
        {
            var mock = new Mock<IBus>();

            var messagecontext = new MessageContext(mock.Object);

            if(route==null)
            {
                messagecontext.SetRoute(new Route<object, Handler>("route", typeof(Handler), new List<Channel>()));
            }
            else
            {
                messagecontext.SetRoute(route);
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

        public static Route<object, Handler> CreateRoute()
        {
            var route = new Route<object, Handler>("route", typeof(Handler), new List<Channel>());

            return route;
        }

        public static Route<object, Handler> CreateRouteWithConsumer(string status=null)
        {
            var route = CreateRoute();

            var method = new RouteMethod<object, Handler>((o,h)=>Task.CompletedTask, status);

            route.RouteMethods.Add(method);

            return route;
        }

        public static Route<object, Handler> CreateRouteWithConsumer(Func<object, Handler, Task> consumer)
        {
            var route = CreateRoute();

            var method = new RouteMethod<object, Handler>(consumer);

            route.RouteMethods.Add(method);

            return route;
        }

        public static Route<object, Handler> CreateRouteWithConsumer(Func<object, Handler, MessageContext, Task> consumer)
        {
            var route = CreateRoute();

            var method = new RouteMethod<object, Handler>(consumer);

            route.RouteMethods.Add(method);

            return route;
        }

        public static Route<object, Handler> CreateRouteWithConsumer(Func<object, Handler, object, Task> consumer)
        {
            var route = CreateRoute();

            var method = new RouteMethod<object, Handler>(consumer);

            route.RouteMethods.Add(method);

            return route;
        }

        public static Route<object, Handler> CreateRouteWithConsumer(Func<object, Handler, MessageContext, object, Task> consumer)
        {
            var route = CreateRoute();

            var method = new RouteMethod<object, Handler>(consumer);

            route.RouteMethods.Add(method);

            return route;
        }


        public static Mock<IComponentFactoryGateway> CreateFactoryMock()
        {
            var factorymock = new Mock<IComponentFactoryGateway>();

            factorymock.Setup(m => m.CreateRouterInterceptor()).Returns(new NullRouterInterceptor());

            factorymock.Setup(x => x.CreateMessageSerializer()).Returns(new NullMessageSerializer());

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