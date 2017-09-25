using Jal.Router.Model;

namespace Jal.Router.AzureStorage.Extensions
{
    public static class SagaSettingExtensions
    {
        public static void SetId(this SagaSetting saga, string partitionkey, string rowkey)
        {
            saga.Id = $"{partitionkey}@{rowkey}";
        }

        public static string GetPartitionKey(this SagaSetting saga)
        {
            if (!string.IsNullOrWhiteSpace(saga.Id))
            {
                var parts = saga.Id.Split('@');

                if (parts.Length == 2)
                {
                    return parts[0];
                }
            }

            return string.Empty;
        }

        public static string GetRowKey(this SagaSetting saga)
        {
            if (!string.IsNullOrWhiteSpace(saga.Id))
            {
                var parts = saga.Id.Split('@');

                if (parts.Length == 2)
                {
                    return parts[1];
                }
            }

            return string.Empty;
        }
    }
}