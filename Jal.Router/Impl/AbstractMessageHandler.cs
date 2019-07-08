using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractMessageHandler
    {
        protected readonly IConfiguration Configuration;

        protected readonly IComponentFactoryGateway Factory;

        protected AbstractMessageHandler(IConfiguration configuration, IComponentFactoryGateway factory)
        {
            Configuration = configuration;

            Factory = factory;
        }

        protected async Task CreateMessageEntityAndSave(MessageContext messagecontext)
        {
            if (Configuration.Storage.Enabled)
            {
                try
                {
                    var storage = Factory.CreateEntityStorage();

                    await storage.CreateMessageEntity(messagecontext, messagecontext.ToEntity()).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    if (!Configuration.Storage.IgnoreExceptions)
                    {
                        throw;
                    }
                }
            }
        }
    }
}