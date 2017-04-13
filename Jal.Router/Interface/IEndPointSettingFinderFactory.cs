using System;

namespace Jal.Router.Interface
{
    public interface IEndPointSettingFinderFactory
    {
        IEndPointSettingFinder<TContent> Create<TContent>(Type type);
    }
}