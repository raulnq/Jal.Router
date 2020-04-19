using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public abstract class AbstractMiddleware
    {
        protected readonly IComponentFactoryFacade Factory;

        protected AbstractMiddleware(IComponentFactoryFacade factory)
        {
            Factory = factory;
        }

        protected async Task CreateMessageEntityAndSave(MessageContext messagecontext)
        {
            if (Factory.Configuration.Storage.Enabled)
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
                    if (!Factory.Configuration.Storage.IgnoreExceptions)
                    {
                        throw;
                    }
                }
            }
        }
    }
}