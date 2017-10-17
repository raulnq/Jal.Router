using System;

namespace Jal.Router.Interface
{
    public interface IComponentFactory
    {
        TComponent Create<TComponent>(Type type) where TComponent : class;
    }
}