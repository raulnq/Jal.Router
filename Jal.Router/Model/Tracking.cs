namespace Jal.Router.Model
{
    public class Tracking
    {
        public string Id { get; private set; }
        public string SagaId { get; private set; }
        public string From { get; private set; }
        public string Key { get; private set; }
        public string RouteName { get; private set; }
        public string SagaName { get; private set; }

        private Tracking()
        {

        }

        public Tracking(string id, string sagaid, string from, string key, string routename, string saganame)
        {
            Id = id;
            SagaId = sagaid;
            From = from;
            Key = key;
            RouteName = routename;
            SagaName = saganame;
        }
    }
}