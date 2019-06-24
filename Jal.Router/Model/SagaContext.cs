namespace Jal.Router.Model
{
    public class SagaContext
    {
        public string Id { get; set; }

        public string Status { get; set; }

        public object Data { get; set; }

        public string ParentId { get; set; }
    }
}