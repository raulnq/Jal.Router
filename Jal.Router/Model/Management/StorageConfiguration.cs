namespace Jal.Router.Model.Management
{
    public class StorageConfiguration
    {
        public bool OptimisticConcurrency { get; set; }
        public bool SaveMessage { get; set; }

        public StorageConfiguration()
        {
            OptimisticConcurrency = true;
            SaveMessage = true;
        }
    }
}