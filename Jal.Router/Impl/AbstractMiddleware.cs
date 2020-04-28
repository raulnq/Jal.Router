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

        protected async Task CreateAndInsertMessageIntoStorage(MessageContext context)
        {
            if (Factory.Configuration.Storage.Enabled)
            {
                try
                {
                    await context.CreateAndInsertMessageIntoStorage(context.ToEntity()).ConfigureAwait(false);
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