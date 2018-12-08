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
                context.Data.AddTrack(context.Data.IdentityContext, context.Data.Origin, context.Data.Route, context.Data.Saga, context.Data.SagaContext);

                CreateMessageEntity(context.Data, MessageEntityType.Inbound, sagaentity);

                var serializer = Factory.Create<IMessageSerializer>(Configuration.MessageSerializerType);

                var data = serializer.Deserialize(sagaentity.Data, context.Data.Saga.DataType);

                if (data != null)
                {
                    _router.Route(context.Data, data);

                    sagaentity.Updated = context.Data.DateTimeUtc;

                    sagaentity.Data = serializer.Serialize(data);

                    UpdateSagaEntity(context.Data, sagaentity);
                }
                else
                {
                    throw new ApplicationException($"Empty/Invalid saga record data {context.Data.Saga.DataType.FullName}, saga {context.Data.Saga.Name} route {context.Data.Route.Name}");
                }
            }
            else
            {
                throw new ApplicationException($"No saga record type {context.Data.Saga.DataType.FullName}, saga {context.Data.Saga.Name} route {context.Data.Route.Name}");
            }
        }
    }
}