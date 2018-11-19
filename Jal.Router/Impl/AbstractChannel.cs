using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;
using Jal.Router.Model.Outbound;

namespace Jal.Router.Impl
{

    public abstract class AbstractChannel
    {
        protected readonly IComponentFactory Factory;

        protected readonly IConfiguration Configuration;

        protected readonly ILogger Logger;

        protected AbstractChannel( IComponentFactory factory, IConfiguration configuration, ILogger logger)
        {
            Factory = factory;
            Configuration = configuration;
            Logger = logger;
        }

        public void OnMessage(ListenerMetadata metadata, string messageid, Action @action, Action completeaction)
        {
            Logger.Log($"Message {messageid} arrived to {metadata.ToString()} channel {metadata.GetPath()}");

            try
            {
                action();

                completeaction();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {messageid} failed to {metadata.ToString()} channel {metadata.GetPath()} {ex}");
            }
            finally
            {
                Logger.Log($"Message {messageid} completed to {metadata.ToString()} channel {metadata.GetPath()}");
            }
        }

        public async Task OnMessageAsync(ListenerMetadata metadata, string messageid, Action @action, Func<Task> completeaction)
        {
            Logger.Log($"Message {messageid} arrived to {metadata.ToString()} channel {metadata.GetPath()}");

            try
            {
                action();

                await completeaction();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {messageid} failed to {metadata.ToString()} channel {metadata.GetPath()} {ex}");
            }
            finally
            {
                Logger.Log($"Message {messageid} completed to {metadata.ToString()} channel {metadata.GetPath()}");
            }
        }
    }
}