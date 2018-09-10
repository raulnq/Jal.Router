using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class OnRouteEntryBuilder : IOnRouteEntryBuilder
    {
        private readonly Route _route;
        public OnRouteEntryBuilder(Route route)
        {
            _route = route;
        }


        public IOnRouteEntryBuilder BuildIdWith(Func<MessageContext, string> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            _route.IdentityConfiguration.IdBuilder = builder;
            return this;
        }

        public IOnRouteEntryBuilder BuildOperationIdWith(Func<MessageContext, string> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            _route.IdentityConfiguration.OperationIdBuilder = builder;
            return this;
        }

        public IOnRouteEntryBuilder BuildParentIdWith(Func<MessageContext, string> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            _route.IdentityConfiguration.ParentIdBuilder = builder;
            return this;
        }

        public IOnRouteEntryBuilder IgnoreExceptionOnSaveMessage(bool ignore)
        {
            _route.StorageConfiguration.IgnoreExceptionOnSaveMessage = ignore;
            return this;
        }

        public IOnRouteEntryBuilder SaveMessage(bool save)
        {
            _route.StorageConfiguration.SaveMessage = save;
            return this;
        }
    }
}