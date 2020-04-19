using Castle.Windsor;
using Jal.Router.Interface;
using System;

namespace Jal.Router.Installer
{
    public static class WindsorContainerExtensions
    {
        public static void AddAzureStorageForRouter(this IWindsorContainer container, Action<IRouterBuilder> action=null)
        {
            container.Install(new RouterInstaller(action));
        }
    }
}
