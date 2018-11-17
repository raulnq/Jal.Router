using System;

namespace Jal.Router.Model.Inbound
{
    public class RouteMetadata
    {
        public Route Route { get; set; }

        public Action<object> Handler { get; set; }

        public string Name { get; set; }
    }
}