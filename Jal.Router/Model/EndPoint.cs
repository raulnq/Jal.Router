namespace Jal.Router.Model
{
    public class EndPoint
    {
        public string Uri { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public bool Enabled { get; set; }

        public int Timeout { get; set; }

        public EndPoint()
        {
            Enabled = true;
        }
    }
}