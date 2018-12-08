namespace Jal.Router.Model
{
    public class Storage
    {
        public bool Enabled { get; set; }
        public bool IgnoreExceptions { get; set; }

        public Storage()
        {
            Enabled = true;
            IgnoreExceptions = false;
        }
    }
}