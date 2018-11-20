using System;
using Jal.Router.Interface;

namespace Jal.Router.Impl.ValueFinder
{
    public class EnvironmentValueFinder : IValueFinder
    {
        public string Find(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
    }
}