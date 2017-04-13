using System;

namespace Jal.Router.Interface
{
    public interface IValueSettingFinderFactory
    {
        IValueSettingFinder Create(Type type);
    }
}