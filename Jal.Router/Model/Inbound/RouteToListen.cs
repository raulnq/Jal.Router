namespace Jal.Router.Model.Inbound
{
    public class RouteToListen
    {
        public Route Route { get; set; }

        public Saga Saga { get; set; }

        public bool IsStart { get; set; }

        public bool IsEnd { get; set; }

        public bool IsNext { get; set; }
    }
}