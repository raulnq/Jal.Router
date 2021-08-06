using Jal.Router.SqlServer.Impl;
using Jal.Router.SqlServer.Model;
using Jal.Router.Interface;
using Jal.Router.Impl;

namespace Jal.Router.SqlServer
{
    public static class ConfigurationExtensions
    {
        public static IConfiguration AddSqlServerAsDefaultTransport(this IConfiguration configuration, SqlServerChannelConnection connection =null)
        {
            var p = new SqlServerChannelConnection();

            if (connection != null)
            {
                p = connection;
            }

            return configuration
                .SetDefaultTransportName("Sql Server")
                .UsePointToPointChannel<SqlServerQueue>()
                .UsePublishSubscribeChannel<SqlServerTopic>()
                .UseSubscriptionToPublishSubscribeChannel<SqlServerSubscription>()
                .UseMessageAdapter<MessageAdapter>()
                .AddParameter(p);
        }


    }
}