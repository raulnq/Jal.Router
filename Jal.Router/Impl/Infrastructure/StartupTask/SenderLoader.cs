﻿using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class SenderLoader : AbstractStartupTask, IStartupTask
    {
        private ISenderContextLoader _loader;

        public SenderLoader(IComponentFactoryGateway factory, ISenderContextLoader loader, ILogger logger)
            : base(factory, logger)
        {
            _loader = loader;
        }

        public Task Run()
        {
            Logger.Log("Loading senders");

            foreach (var endpoint in Factory.Configuration.Runtime.EndPoints)
            {
                foreach (var channel in endpoint.Channels)
                {
                    var sendercontext = Factory.Configuration.Runtime.SenderContexts.FirstOrDefault(x => x.Channel.Id == channel.Id);

                    if (sendercontext == null)
                    {
                        sendercontext =_loader.Load(channel);
                    }

                    sendercontext.Endpoints.Add(endpoint);
                }
            }

            Logger.Log("Senders loaded");

            return Task.CompletedTask;
        }
    }
}