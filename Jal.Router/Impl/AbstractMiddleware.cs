using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractMiddleware
    {
        protected readonly IConfiguration Configuration;

        protected readonly IComponentFactoryGateway Factory;

        protected AbstractMiddleware(IConfiguration configuration, IComponentFactoryGateway factory)
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

                    var entity = messagecontext.ToEntity();

                    var id = await storage.Create(entity).ConfigureAwait(false);

                    entity.SetId(id);
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