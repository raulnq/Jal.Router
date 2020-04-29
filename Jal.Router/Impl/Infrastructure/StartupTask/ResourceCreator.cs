using System;
using System.Text;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ResourceCreator : AbstractStartupTask, IStartupTask
    {
        public ResourceCreator(IComponentFactoryFacade factory, ILogger logger) 
            : base(factory, logger)
        {

        }

        public async Task Run()
        {
            var errors = new StringBuilder();

            Logger.Log("Creating resources");

            foreach (var context in Factory.Configuration.Runtime.ResourceContexts)
            {
                try
                {
                    var created = await context.CreateIfNotExist().ConfigureAwait(false);

                    if (created)
                    {
                        Logger.Log($"Created {context.Resource.FullPath} {context.Resource.ToString()} resource");
                    }
                    else
                    {
                        Logger.Log($"The {context.Resource.ToString()} resource {context.Resource.FullPath} already exists");
                    }
                }
                catch (Exception ex)
                {
                    var error = $"Exception {context.Resource.FullPath} {context.Resource.ToString()} resource: {ex}";

                    errors.AppendLine(error);

                    Logger.Log(error);
                }
            }

            if (!string.IsNullOrWhiteSpace(errors.ToString()))
            {
                throw new ApplicationException(errors.ToString());
            }

            Logger.Log("Resources created");
        }
    }
}