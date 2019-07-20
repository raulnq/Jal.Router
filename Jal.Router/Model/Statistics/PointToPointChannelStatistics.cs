using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class PointToPointChannelStatistics
    {
        public Dictionary<string, string> Properties { get; }

        public PointToPointChannelStatistics(string path)
        {
            Path = path;

            Properties = new Dictionary<string, string>();
        }

        public string Path { get; }
    }
}