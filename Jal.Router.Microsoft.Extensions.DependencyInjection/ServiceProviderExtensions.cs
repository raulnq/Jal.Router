using Jal.Router.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Jal.Router.Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static IHost GetRouterHost(this IServiceProvider provider)
        {
            return provider.GetService<IHost>();
        }

        public static IBus GetRouterBus(this IServiceProvider provider)
        {
            return provider.GetService<IBus>();
        }
    }
}
