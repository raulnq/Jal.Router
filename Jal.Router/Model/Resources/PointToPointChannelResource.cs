using Jal.Router.Interface;
using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{

    public class PointToPointChannelResource : Resource
    {
        public PointToPointChannelResource(string path, string connectionstring, Dictionary<string, string> properties)
            :base(path, connectionstring, properties)
        {
        }
    }
}