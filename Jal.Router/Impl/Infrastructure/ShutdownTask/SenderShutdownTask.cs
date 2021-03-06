﻿using Jal.Router.Interface;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class SenderShutdownTask : IShutdownTask
    {
        private readonly ILogger _logger;

        private readonly IConfiguration _configuration;

        public SenderShutdownTask(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Run()
        {
            foreach (var context in _configuration.Runtime.SenderContexts)
            {
                await context.Close().ConfigureAwait(false);

                _logger.Log($"Shutdown {context.ToString()}");
            }
        }
    }
}