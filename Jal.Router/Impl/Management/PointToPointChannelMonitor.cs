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

                var manager = _factory.Create<IChannelManager>(_configuration.ChannelManagerType);

                foreach (var source in _sources)
                {
                    var channels = source.GetPointToPointChannels();

                    foreach (var channel in channels)
                    {
                        var info = manager.GetPointToPointChannel(channel.ConnectionString, channel.Path);

                        if (info != null)
                        {
                            Array.ForEach(loggers, x=> x.Log(info, datetime));
                        }
                    }
                }
            }
        }
    }
}