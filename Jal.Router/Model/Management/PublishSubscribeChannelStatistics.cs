using System.Collections.Generic;

namespace Jal.Router.Model.Management
{
    public class PublishSubscribeChannelStatistics
    {
        public PublishSubscribeChannelStatistics(string path)
        {
            Path = path;

            Properties = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Properties { get; }

        public string Path { get; }
    }
}