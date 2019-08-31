﻿using System.Threading.Tasks;

namespace Jal.Router.Interface
{
    public interface IHost
    {
        void RunAndBlock();

        void Run();

        Task Startup();

        Task Shutdown();

        IConfiguration Configuration { get; }
    }
}