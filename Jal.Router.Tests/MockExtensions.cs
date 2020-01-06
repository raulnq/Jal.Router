using Jal.Router.Interface;
using Jal.Router.Model;
using Moq;

namespace Jal.Router.Tests
{
    public static class MockExtensions
    {
        public static void WasExecuted(this Mock<IConsumer> consumermock)
        {
            consumermock.Verify(x => x.Consume(It.IsAny<MessageContext>()), Times.Once());
        }

        public static void WasNotExecuted(this Mock<IConsumer> consumermock)
        {
            consumermock.Verify(x => x.Consume(It.IsAny<MessageContext>()), Times.Never());
        }

        public static void CreateEntityStorageWasNotExecuted(this Mock<IComponentFactoryGateway> factorymock)
        {
            factorymock.Verify(x => x.CreateEntityStorage(), Times.Never());
        }

        public static void CreateEntityStorageWasExecuted(this Mock<IComponentFactoryGateway> factorymock)
        {
            factorymock.Verify(x => x.CreateEntityStorage(), Times.Once());
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
