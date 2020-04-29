using Jal.Router.Interface;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class RuntimeLoader : AbstractStartupTask, IStartupTask
    {
        private readonly IRouterConfigurationSource[] _sources;

        public RuntimeLoader(IComponentFactoryFacade factory, IRouterConfigurationSource[] sources, ILogger logger)
            :base(factory, logger)
        {
            _sources = sources;
        }

        public Task Run()
        {
            Logger.Log("Loading runtime configuration");

            foreach (var source in _sources)
            {
                Factory.Configuration.Runtime.EndPoints.AddRange(source.GetEndPoints());

                Factory.Configuration.Runtime.Resources.AddRange(source.GetResources());

                Factory.Configuration.Runtime.Sagas.AddRange(source.GetSagas());

                foreach (var route in source.GetRoutes())
                {
                    Factory.Configuration.Runtime.Routes.Add(route);
                }
            }

            foreach (var saga in Factory.Configuration.Runtime.Sagas)
            {
                if (saga.InitialRoutes != null)
                {
                    foreach (var route in saga.InitialRoutes)
                    {
                        Factory.Configuration.Runtime.Routes.Add(route);
                    }
                }

                if (saga.FinalRoutes != null)
                {
                    foreach (var route in saga.FinalRoutes)
                    {
                        Factory.Configuration.Runtime.Routes.Add(route);
                    }
                }

                if(saga.Routes!=null)
                {
                    foreach (var route in saga.Routes)
                    {
                        Factory.Configuration.Runtime.Routes.Add(route);
                    }
                }
            }

            Logger.Log("Runtime configuration loaded");

            return Task.CompletedTask;
        }
    }
}