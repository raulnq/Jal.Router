using System;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class EnvironmentSettingFinder : IValueFinder
    {
        public string Find(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}