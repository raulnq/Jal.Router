using System;
using System.Threading.Tasks;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

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
            Logger.Log($"Message {messageid} arrived to {metadata.Channel.ToString()} channel {metadata.Channel.GetPath()}");

            try
            {
                action();

                completeaction();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {messageid} failed to {metadata.Channel.ToString()} channel {metadata.Channel.GetPath()} {ex}");
            }
            finally
            {
                Logger.Log($"Message {messageid} completed to {metadata.Channel.ToString()} channel {metadata.Channel.GetPath()}");
            }
        }

        public async Task OnMessageAsync(ListenerMetadata metadata, MessageContext context, Action @action, Func<Task> completeaction)
        {
            Logger.Log($"Message {context.IdentityContext.Id} arrived to {metadata.Channel.ToString()} channel {metadata.Channel.GetPath()}");

            try
            {
                action();

                await completeaction();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {context.IdentityContext.Id} failed to {metadata.Channel.ToString()} channel {metadata.Channel.GetPath()} {ex}");
            }
            finally
            {
                Logger.Log($"Message {context.IdentityContext.Id} completed to {metadata.Channel.ToString()} channel {metadata.Channel.GetPath()}");
            }
        }

        public async Task OnMessageAsync(ListenerMetadata metadata, MessageContext context, Action @action, Func<Task> completeaction, Func<Task> completegroup)
        {
            Logger.Log($"Message {context.IdentityContext.Id} arrived to {metadata.Channel.ToString()} channel {metadata.Channel.GetPath()}");

            try
            {
                action();

                await completeaction();

                if(metadata.Group.Until(context))
                {
                    await completegroup();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {context.IdentityContext.Id} failed to {metadata.Channel.ToString()} channel {metadata.Channel.GetPath()} {ex}");
            }
            finally
            {
                Logger.Log($"Message {context.IdentityContext.Id} completed to {metadata.Channel.ToString()} channel {metadata.Channel.GetPath()}");
            }
        }
    }
}