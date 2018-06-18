using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class EndPoint
    {
        public EndPoint(string name)
        {
            Name = name;
            Channels = new List<Channel>();
            MiddlewareTypes = new List<Type>();
        }
        public Origin Origin { get; set; }

        public string Name { get; set; }

        public Type MessageType { get; set; }

        public IList<Channel> Channels { get; set; }
        public IList<Type> MiddlewareTypes { get; set; }
    }
}