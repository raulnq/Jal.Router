namespace Jal.Router.Model
{
    public class InboundMessageContext
    {
        public string Id { get; set; }
        public string ReplyToConnectionString { get; set; }//TODO delete
        public string ReplyToPath { get; set; }//TODO delete
        public string From { get; set; }
        public string Origin { get; set; }
        public string ReplyTo { get; set; }//TODO delete
    }
}
