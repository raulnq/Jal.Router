namespace Jal.Router.Model
{
    public class EndPointSetting
    {
        public string ToConnectionString { get; set; }
        public string ToPath { get; set; }
        public string ReplyToConnectionString { get; set; }
        public string ReplyToPath { get; set; }
        public string From { get; set; }
    }
}