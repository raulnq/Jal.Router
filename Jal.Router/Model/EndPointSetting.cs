namespace Jal.Router.Model
{
    public class EndPointSetting
    {
        public string ToConnectionString { get; set; }
        public string ToPath { get; set; }
        public string ReplyToConnectionString { get; set; }//TODO delete
        public string ReplyToPath { get; set; }//TODO delete
        public string From { get; set; }
        public string Origin { get; set; }
    }
}