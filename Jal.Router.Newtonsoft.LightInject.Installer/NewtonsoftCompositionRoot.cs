using Jal.Router.Interface;
using Jal.Router.Newtonsoft.Impl;
using LightInject;

namespace Jal.Router.Newtonsoft.LightInject.Installer
{
    public class NewtonsoftCompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IMessageSerializer, JsonMessageSerializer>(typeof(JsonMessageSerializer).FullName, new PerContainerLifetime());
        }
    }
}
