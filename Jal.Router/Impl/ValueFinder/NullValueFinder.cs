using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class NullValueFinder : IValueFinder
    {
        public string Find(string name)
        {
            return string.Empty;
        }
    }
}