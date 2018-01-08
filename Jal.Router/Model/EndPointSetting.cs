namespace Jal.Router.Model
{
    public class EndPointSetting
    {
        public string ToConnectionString { get; set; }
        public string ToPath { get; set; }
        public string ToReplyConnectionString { get; set; }
        public string ToReplyPath { get; set; }
        public string ToReplySubscription { get; set; }
        public int ToReplyTimeOut { get; set; }
        public string EndPointName { get; set; }
    }
}