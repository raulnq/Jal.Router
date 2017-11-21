﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Outbound;
using Jal.Router.Logger.Impl;
using Jal.Router.Model.Management;

namespace Jal.Router.Logger.Installer
{
    public class RouterLoggerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<Jal.Router.Interface.Outbound.IMiddleware>().ImplementedBy<BusLogger>().Named(typeof(BusLogger).FullName).LifestyleSingleton());
            container.Register(Component.For<Jal.Router.Interface.Inbound.IMiddleware>().ImplementedBy<RouterLogger>().Named(typeof(RouterLogger).FullName).LifestyleSingleton());
            container.Register(Component.For<ILogger<HeartBeat>>().ImplementedBy<HeartBeatLogger>().Named(typeof(HeartBeatLogger).FullName).LifestyleSingleton());
        }
    }
}
