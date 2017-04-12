using System;
using Jal.Router.Interface;

namespace Jal.Router.Fluent.Interface
{
    public interface IPolicyRetryBuilder
    {
        void UsePolicy(Func<IValueSettingFinder, IRetryPolicy> policycreator);
    }
}