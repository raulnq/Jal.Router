namespace Jal.Router.Model
{
    public class SagaContextEntity
    {
        public SagaData SagaData { get; private set; }

        private SagaContextEntity()
        {

        }

        public SagaContextEntity(SagaData sagadata)
        {
            SagaData = sagadata;
        }
    }
}
