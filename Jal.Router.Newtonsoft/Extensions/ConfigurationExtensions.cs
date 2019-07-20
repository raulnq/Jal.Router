using Jal.Router.Interface;
using Jal.Router.Newtonsoft.Impl;
using Jal.Router.Newtonsoft.Model;

namespace Jal.Router.Newtonsoft.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration UseNewtonsoft(this IConfiguration configuration, NewtonsoftSerializerParameter parameter =null)
        {

            if(parameter==null)
            {
                parameter = new NewtonsoftSerializerParameter();
            }

            return configuration
                .UseMessageSerializer<JsonMessageSerializer>()
                .AddParameter(parameter);
        }


    }
}