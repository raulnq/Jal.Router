﻿using System;
using System.Threading;
using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class CtrlCShutdownWatcher : IShutdownWatcher
    {
        private readonly ILogger _logger;

        public CtrlCShutdownWatcher(ILogger logger)
        {
            _logger = logger;
        }

        public void Start(CancellationTokenSource tokensource)
        {
            Console.CancelKeyPress += (sender, e) =>
            {
                e.Cancel = true;

                if (tokensource != null && !tokensource.IsCancellationRequested)
                {
                    tokensource.Cancel();
                }  
            };

            _logger.Log("Press Ctrl+C to shut down");
        }

        public void Stop()
        {

        }
    }
}