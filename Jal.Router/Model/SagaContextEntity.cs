namespace Jal.Router.Model
{
    public class SagaContextEntity
    {
        public SagaData SagaData { get; }

        public SagaContextEntity()
        {

        }

        public SagaContextEntity(SagaData sagadata)
        {
            SagaData = sagadata;
        }
    }
}
