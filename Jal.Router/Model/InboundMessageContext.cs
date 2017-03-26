namespace Jal.Router.Model
{
    public class InboundMessageContext
    {
        public string Id { get; set; }
        public string ReplyToConnectionString { get; set; }
        public string ReplyToPath { get; set; }
        public string From { get; set; }
    }
}
