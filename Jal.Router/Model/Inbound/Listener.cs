using System;

namespace Jal.Router.Model.Inbound
{
    public class Listener
    {
        public Route Route { get; set; }

        public Action<object> Action { get; set; }

        public string Prefix { get; set; }
    }
}