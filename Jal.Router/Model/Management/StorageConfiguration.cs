namespace Jal.Router.Model.Management
{
    public class StorageConfiguration
    {
        public bool OptimisticConcurrency { get; set; }

        public bool ManualSagaSave { get; set; }

        public bool ManualMessageSave { get; set; }

        public StorageConfiguration()
        {
            OptimisticConcurrency = true;
            ManualMessageSave = false;
            ManualSagaSave = false;
        }
    }
}