using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Statistic
    {
        public Statistic(string path, string subscription = null)
        {
            Subscription = subscription;
            Path = path;
            Properties = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Properties { get; }

        public string Subscription { get; }

        public string Path { get; }
    }
}