﻿using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Serilog
{
    public static class RouterBuilderExtensions
    {
        public static void AddSerilog(this IRouterBuilder builder)
        {
            builder.AddMiddleware<BusLogger>();

            builder.AddMiddleware<RouterLogger>();

            builder.AddLogger<BeatLogger, Beat>();
        }
    }
}
