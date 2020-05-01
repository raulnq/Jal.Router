using System.Collections.Generic;
using Jal.Router.AzureServiceBus.Standard.Model;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.AzureServiceBus.Standard
{
    public static class OptionBuilderExtensions
    {
        public static void CreateIfNotExist(this IOptionBuilder builder, AzureServiceBusChannelProperties properties, string filter = null)
        {
            Rule r = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                r = new Rule(filter, "$Default", true);
            }

            var dictionary = new Dictionary<string, object>();

            if (properties.DefaultMessageTtlInDays > 0)
            {
                dictionary.Add("defaultmessagettlindays", properties.DefaultMessageTtlInDays.ToString());
            }
            if (properties.DuplicateMessageDetectionInMinutes > 0)
            {
                dictionary.Add("duplicatemessagedetectioninminutes", properties.DuplicateMessageDetectionInMinutes.ToString());
            }
            if (properties.ExpressMessageEnabled != null)
            {
                dictionary.Add("expressmessageenabled", "true");
            }
            if (properties.MessageLockDurationInSeconds > 0)
            {
                dictionary.Add("messagelockdurationinseconds", properties.MessageLockDurationInSeconds.ToString());
            }
            if (properties.PartitioningEnabled != null)
            {
                dictionary.Add("partitioningenabled", "true");
            }
            if (properties.SessionEnabled != null)
            {
                dictionary.Add("sessionenabled", "true");
            }

            builder.CreateIfNotExist(dictionary, new List<Rule>() { r });
        }

        public static void CreateIfNotExist(this IOptionBuilder builder, string filter = null)
        {

            Rule r = null;

            if (!string.IsNullOrWhiteSpace(filter))
            {
                r = new Rule(filter, "$Default", true);
            }

            builder.CreateIfNotExist(rules: new List<Rule>() { r });
        }
    }
}