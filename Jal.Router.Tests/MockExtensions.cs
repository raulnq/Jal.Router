using Jal.Router.Interface;
using Jal.Router.Model;
using Moq;
using System;
using System.Linq.Expressions;

namespace Jal.Router.Tests
{
    public static class MockExtensions
    {
        public static void WasExecuted(this Mock<IProducer> producermock)
        {
            producermock.Verify(x => x.Produce(It.IsAny<MessageContext>()), Times.Once());
        }

        public static void WasNotExecuted(this Mock<IProducer> producermock)
        {
            producermock.Verify(x => x.Produce(It.IsAny<MessageContext>()), Times.Never());
        }

        public static void WasExecuted(this Mock<IConsumer> consumermock)
        {
            consumermock.Verify(x => x.Consume(It.IsAny<MessageContext>()), Times.Once());
        }

        public static void WasNotExecuted(this Mock<IConsumer> consumermock)
        {
            consumermock.Verify(x => x.Consume(It.IsAny<MessageContext>()), Times.Never());
        }

        public static void CreateEntityStorageWasNotExecuted(this Mock<IComponentFactoryFacade> factorymock)
        {
            factorymock.Verify(x => x.CreateEntityStorage(), Times.Never());
        }

        public static void CreateEntityStorageWasExecuted(this Mock<IComponentFactoryFacade> factorymock)
        {
            factorymock.Verify(x => x.CreateEntityStorage(), Times.Once());
        }

        public static void CreateMessageAdapterWasExecuted(this Mock<IComponentFactoryFacade> factorymock)
        {
            factorymock.Verify(x => x.CreateMessageAdapter(It.IsAny<Type>()), Times.Once());
        }

        public static void WasExecuted(this Mock<ISenderChannel> sendermock)
        {
            sendermock.Verify(x => x.Send(It.IsAny<SenderContext>(), It.IsAny<object>()), Times.Once());
        }

        public static void CreateMessageSerializerWasExecuted(this Mock<IComponentFactoryFacade> factorymock)
        {
            factorymock.Verify(x => x.CreateMessageSerializer(), Times.Once());
        }

        public static void SendWasExecuted<T>(this Mock<IBus> busmock)
        {
            busmock.Verify(x => x.Send<T>(It.IsAny<T>(), It.IsAny<Options>()), Times.Once());

            //busmock.Verify(x => x.Send<T>(It.IsAny<T>(), It.IsAny<Origin>(), It.IsAny<Options>()), Times.AtMostOnce());

            //busmock.Verify(x => x.Send<T>(It.IsAny<T>(), It.IsAny<EndPoint>(), It.IsAny<Origin>(), It.IsAny<Options>()), Times.AtMostOnce());
        }

        public static void SendWasNotExecuted<T>(this Mock<IBus> busmock)
        {
            busmock.Verify(x => x.Send<T>(It.IsAny<T>(), It.IsAny<Options>()), Times.Never());
        }

        public static void SendWasExecuted<T>(this Mock<IBus> busmock, Expression<Func<Options, bool>> match)
        {
            busmock.Verify(x => x.Send<T>(It.IsAny<T>(), It.Is<Options>(match)), Times.Once());
        }

        public static void CreateMessageSerializerWasNotExecuted(this Mock<IComponentFactoryFacade> factorymock)
        {
            factorymock.Verify(x => x.CreateMessageSerializer(), Times.Never());
        }

        public static void CreateMessageEntityWasExecuted(this Mock<IEntityStorage> entitystoragemock)
        {
            entitystoragemock.Verify(x => x.Create(It.IsAny<MessageEntity>()), Times.Once());
        }

        public static void CreateMessageEntityWasNotExecuted(this Mock<IEntityStorage> entitystoragemock)
        {
            entitystoragemock.Verify(x => x.Create(It.IsAny<MessageEntity>()), Times.Never());
        }

        public static void CreateSagaDataWasExecuted(this Mock<IEntityStorage> entitystoragemock)
        {
            entitystoragemock.Verify(x => x.Create(It.IsAny<SagaData>()), Times.Once());
        }

        public static void UpdateSagaDataWasExecuted(this Mock<IEntityStorage> entitystoragemock)
        {
            entitystoragemock.Verify(x => x.Update(It.IsAny<SagaData>()), Times.Once());
        }

        public static void UpdateSagaDataWasNotExecuted(this Mock<IEntityStorage> entitystoragemock)
        {
            entitystoragemock.Verify(x => x.Update(It.IsAny<SagaData>()), Times.Never());
        }

        public static void GetSagaDataWasExecuted(this Mock<IEntityStorage> entitystoragemock)
        {
            entitystoragemock.Verify(x => x.Get(It.IsAny<string>()), Times.Once());
        }
    }
}
