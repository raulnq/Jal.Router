using System;

namespace Jal.Router.Model
{
    public class SagaContext
    {
        public SagaData SagaData { get; private set; }

        public MessageContext Context { get; }

        public string Id { get; private set; }

        public SagaContext(MessageContext context, string id)
        {
            Id = id;

            Context = context;
        }

        public SagaContext()
        {

        }

        public void UpdateSagaData(SagaData sagadata)
        {
            SagaData = sagadata;
            UpdateId(sagadata.Id);
        }

        public void UpdateId(string id)
        {
            Id = Id;
        }

        public SagaData CreateSagaData(string status)
        {
            var data = Activator.CreateInstance(Context.Saga.DataType);

            var entity = new SagaData(data, Context.Saga.DataType.FullName, Context.Saga.Name, Context.DateTimeUtc, Context.Saga.Timeout, status);

            return entity;
        }

        public SagaContextEntity ToEntity()
        {
            return new SagaContextEntity(SagaData);
        }
    }
}