namespace Jal.Router.Model.Management
{
    public class StorageConfiguration
    {
        public bool SaveMessage { get; set; }
        public bool IgnoreExceptionOnSaveMessage { get; set; }

        public StorageConfiguration()
        {
            SaveMessage = true;
            IgnoreExceptionOnSaveMessage = false;
        }
    }
}