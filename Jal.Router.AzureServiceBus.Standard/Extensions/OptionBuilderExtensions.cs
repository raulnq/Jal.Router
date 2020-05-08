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
            if (!string.IsNullOrWhiteSpace(filter))
            {
                var r = new Rule(filter, "$Default", true);

                builder.CreateIfNotExist(properties?.ToDictionary(), new List<Rule>() { r });
            }
            else
            {
                builder.CreateIfNotExist(properties?.ToDictionary());
            }
        }

        public static void CreateIfNotExist(this IOptionBuilder builder, string filter = null)
        {
            if (!string.IsNullOrWhiteSpace(filter))
            {
                var r = new Rule(filter, "$Default", true);

                builder.CreateIfNotExist(rules: new List<Rule>() { r });
            }
            else
            {
                builder.CreateIfNotExist();
            }
        }
    }
}