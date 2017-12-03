using Jal.Router.Model;

namespace Jal.Router.AzureStorage.Extensions
{
    public static class SagaSettingExtensions
    {
        public static void SetId(this SagaInfo saga, string partitionkey, string rowkey, string tablenamesufix)
        {
            saga.Id = $"{partitionkey}@{rowkey}@{tablenamesufix}";
        }

        public static string GetPartitionKey(this SagaInfo saga)
        {
            if (!string.IsNullOrWhiteSpace(saga.Id))
            {
                var parts = saga.Id.Split('@');

                if (parts.Length >= 2)
                {
                    return parts[0];
                }
            }

            return string.Empty;
        }

        public static string GetRowKey(this SagaInfo saga)
        {
            if (!string.IsNullOrWhiteSpace(saga.Id))
            {
                var parts = saga.Id.Split('@');

                if (parts.Length >= 2)
                {
                    return parts[1];
                }
            }

            return string.Empty;
        }

        public static string GetTableNameSufix(this SagaInfo saga)
        {
            if (!string.IsNullOrWhiteSpace(saga.Id))
            {
                var parts = saga.Id.Split('@');

                if (parts.Length == 3)
                {
                    return parts[2];
                }
            }

            return string.Empty;
        }
    }
}