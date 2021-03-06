﻿using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.ApplicationInsights
{
    public static class RouterBuilderExtensions
    {
        public static IRouterBuilder AddApplicationInsights(this IRouterBuilder builder)
        {
            builder.AddLogger<BeatLogger, Beat>();

            builder.AddLogger<StatisticsLogger, Statistic>();

            builder.AddMiddleware<RouterLogger>();

            builder.AddMiddleware<BusLogger>();

            return builder;
        }
    }
}
