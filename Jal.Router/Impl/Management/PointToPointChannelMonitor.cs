using System;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model.Management;

namespace Jal.Router.Impl.Management
{
    public class PointToPointChannelMonitor : IMonitoringTask
    {
        private readonly IRouterConfigurationSource[] _sources;

        private readonly IComponentFactory _factory;

        private readonly IConfiguration _configuration;

        public PointToPointChannelMonitor(IRouterConfigurationSource[] sources, IComponentFactory factory, IConfiguration configuration)
        {
            _sources = sources;
            _factory = factory;
            _configuration = configuration;
        }

        public void Run(DateTime datetime)
        {
            if (_configuration.LoggerTypes.ContainsKey(typeof (PointToPointChannelInfo)))
            {
                var loggertypes = _configuration.LoggerTypes[typeof (PointToPointChannelInfo)];

                var loggers = loggertypes.Select(x => _factory.Create<ILogger<PointToPointChannelInfo>>(x)).ToArray();

                var channelmanager = _factory.Create<IChannelManager>(_configuration.ChannelManagerType);

                foreach (var source in _sources)
                {
                    var queues = source.GetPointToPointChannels();

                    foreach (var queue in queues)
                    {
                        var info = GetPointToPointChannel(queue, channelmanager);

                        if (info != null)
                        {
                            Array.ForEach(loggers, x=> x.Log(info, datetime));
                        }
                    }
                }
            }
        }

        public PointToPointChannelInfo GetPointToPointChannel(PointToPointChannel pointToPointChannel, IChannelManager channelmanager)
        {
            if (pointToPointChannel.ConnectionStringExtractorType != null)
            {
                var extractorconnectionstring = _factory.Create<IValueSettingFinder>(pointToPointChannel.ConnectionStringExtractorType);

                var toconnectionextractor = pointToPointChannel.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;

                if (toconnectionextractor != null)
                {
                    return channelmanager.GetPointToPointChannel(toconnectionextractor(extractorconnectionstring), pointToPointChannel.Path);
                }
            }

            return null;
        }
    }
}