namespace Jal.Router.Model.Management
{
    public class StorageConfiguration
    {
        public bool Enabled { get; set; }
        public bool IgnoreExceptions { get; set; }

        public StorageConfiguration()
        {
            Enabled = true;
            IgnoreExceptions = false;
        }
    }
}