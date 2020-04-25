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

            foreach (var resource in Factory.Configuration.Runtime.Resources)
            {
                try
                {
                    var manager = Factory.CreateResourceManager(resource.ChannelType);

                    var created = await manager.CreateIfNotExist(resource).ConfigureAwait(false);

                    if (created)
                    {
                        Logger.Log($"Created {resource.FullPath} {resource.ToString()} resource");
                    }
                    else
                    {
                        Logger.Log($"The {resource.ToString()} resource {resource.FullPath} already exists");
                    }
                }
                catch (Exception ex)
                {
                    var error = $"Exception {resource.FullPath} {resource.ToString()} resource: {ex}";

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