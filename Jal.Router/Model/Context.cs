namespace Jal.Router.Model
{
    public class Context
    {
        public string Id { get; set; }
        public string Correlation { get; set; }
        public string ReplyToConnectionString { get; set; }
        public string ReplyToPath { get; set; }
        public string ReplyToPublisherPath { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
