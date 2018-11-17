using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class NullValueSettingFinder : IValueFinder
    {
        public string Find(string name)
        {
            return string.Empty;
        }
    }
}