﻿using System;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Inbound;
using Jal.Router.Interface.Management;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound
{
    public class MiddleMessageHandler : AbstractMessageHandler, IMiddleware<MessageContext>
    {
        private readonly IMessageRouter _router;
        
        private const string DefaultStatus = "IN PROCESS";

        public MiddleMessageHandler(IComponentFactory factory, IMessageRouter router, IConfiguration configuration) : base(configuration, factory)
        {
            _router = router;
        }

        public void Execute(Context<MessageContext> context, Action<Context<MessageContext>> next)
        {
            context.Data.SagaContext.Status = DefaultStatus;

            var sagaentity = GetSagaEntity(context.Data);

            if (sagaentity != null)
            {
                context.Data.AddTrack(context.Data.Identity, context.Data.Origin, context.Data.Route, context.Data.Saga, context.Data.SagaContext);

                var serializer = Factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);

                var data = serializer.Deserialize(sagaentity.Data, context.Data.Saga.DataType);

                if (data != null)
                {
                    _router.Route(context.Data, data);

                    sagaentity.Updated = context.Data.DateTimeUtc;

                    UpdateSagaEntity(context.Data, sagaentity, data);
                }
                else
                {
                    throw new ApplicationException($"Empty/Invalid data {context.Data.Saga.DataType.FullName} for {context.Data.Route.ContentType.FullName}, saga {context.Data.Saga.Name} route {context.Data.Route.Name}");
                }
            }
            else
            {
                throw new ApplicationException($"No data {context.Data.Saga.DataType.FullName} for {context.Data.Route.ContentType.FullName}, saga {context.Data.Saga.Name} route {context.Data.Route.Name}");
            }
        }
    }
}