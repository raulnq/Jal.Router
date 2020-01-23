using Jal.Router.Interface;
using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{

    public class PointToPointChannelResource : ChannelResource
    {
        public PointToPointChannelResource(string path, string connectionstring, Dictionary<string, string> properties)
            :base(path, connectionstring, properties)
        {
        }

        public PointToPointChannelResource(string path, Dictionary<string, string> properties, Type type, Func<IValueFinder, string> provider)
            :base(path, properties, type, provider)
        {
        }
    }
}