using System;

namespace Jal.Router.Interface
{
    public interface IEndPointSettingFinderFactory
    {
        IValueSettingFinder Create(Type type);

        IEndPointSettingFinder<TContent> Create<TContent>(Type type);
    }
}