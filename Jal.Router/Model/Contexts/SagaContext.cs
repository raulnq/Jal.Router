using System;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class SagaContext
    {
        public SagaData Data { get; private set; }

        public MessageContext Context { get; private set; }

        public string Id { get; private set; }

        public SagaContext(MessageContext context, string id)
        {
            Id = id;

            Context = context;
        }

        private SagaContext()
        {

        }

        public void Load(SagaData sagadata)
        {
            Data = sagadata;
        }

        public bool IsLoaded()
        {
            return Data!=null;
        }

        public void SetId(string id)
        {
            Id = id;
        }

        public SagaData Create(string status)
        {
            var data = Activator.CreateInstance(Context.Saga.DataType);

            var entity = new SagaData(data, Context.Saga.DataType, Context.Saga.Name, Context.DateTimeUtc, Context.Saga.Timeout, status);

            return entity;
        }

        public SagaContextEntity ToEntity()
        {
            return new SagaContextEntity(Data);
        }
    }
}