namespace Jal.Router.AzureServiceBus.Model
{
    public class BrokeredMessageEndPoint
    {
        public string ToConnectionString { get; set; }
        public string To { get; set; }
        public string ReplyToConnectionString { get; set; }
        public string ReplyTo { get; set; }
        public string From { get; set; }
    }
}