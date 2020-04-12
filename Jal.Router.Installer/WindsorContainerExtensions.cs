using Castle.Windsor;
using Jal.Router.Interface;
using System;

namespace Jal.Router.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddAzureStorageForRouter(this IWindsorContainer container, IRouterConfigurationSource[] sources, Action<IWindsorContainer> action=null)
        {
            container.Install(new RouterInstaller(sources, action));
        }
    }
}
