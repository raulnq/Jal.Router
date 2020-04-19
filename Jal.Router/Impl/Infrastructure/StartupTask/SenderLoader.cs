﻿using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class SenderLoader : AbstractStartupTask, IStartupTask
    {
        private readonly ISenderContextLifecycle _lifecycle;

        public SenderLoader(IComponentFactoryFacade factory, ISenderContextLifecycle lifecycle, ILogger logger)
            : base(factory, logger)
        {
            _lifecycle = lifecycle;
        }

        public Task Run()
        {
            Logger.Log("Loading senders");

            Create();

            Open();

            Logger.Log("Senders loaded");

            return Task.CompletedTask;
        }

        private void Open()
        {
            foreach (var sendercontext in Factory.Configuration.Runtime.SenderContexts)
            {
                if(sendercontext.Open())
                {
                    Logger.Log($"Opening {sendercontext.Id}");
                }
            }
        }

        private void Create()
        {
            foreach (var endpoint in Factory.Configuration.Runtime.EndPoints)
            {
                foreach (var channel in endpoint.Channels)
                {
                    var sendercontext = _lifecycle.AddOrGet(channel);

                    sendercontext.Endpoints.Add(endpoint);
                }
            }
        }
    }
}