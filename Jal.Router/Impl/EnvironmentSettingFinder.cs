using System;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class EnvironmentSettingFinder : IValueSettingFinder
    {
        public string Find(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}