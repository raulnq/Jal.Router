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

        public SagaContext(MessageContext context, string id, SagaData sagadata):this(context, id)
        {
            Data = sagadata;
        }

        private SagaContext()
        {

        }

        public bool IsLoaded()
        {
            return Data!=null;
        }

        public void Load(SagaData sagadata)
        {
            Data = sagadata;
        }

        public Task UpdateIntoStorage()
        {
            return Context.UpdateSagaDataIntoStorage(Data);
        }

        public async Task LoadDataFromStorage()
        {
            Load(await Context.GetSagaDataFromStorage(Id).ConfigureAwait(false));
        }

        public async Task CreateAndInsertDataIntoStorage(string status)
        {
            var data = Activator.CreateInstance(Context.Saga.DataType);

            var sagadata = new SagaData(data, Context.Saga.DataType, Context.Saga.Name, Context.DateTimeUtc, Context.Saga.Timeout, status);

            var id = await Context.InsertSagaDataIntoStorage(sagadata).ConfigureAwait(false);

            sagadata.SetId(id);

            Load(sagadata);

            Id = id;
        }

        public SagaContextEntity ToEntity()
        {
            return new SagaContextEntity(Data);
        }
    }
}