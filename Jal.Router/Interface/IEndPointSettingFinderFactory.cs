using System;

namespace Jal.Router.Interface
{
    public interface IEndPointSettingFinderFactory
    {
        IEndPointValueSettingFinder Create(Type type);

        IEndPointSettingFinder<TContent> Create<TContent>(Type type);
    }
}