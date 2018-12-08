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


        public IOnRouteEntryBuilder BuildIdentityWith(Func<MessageContext, IdentityContext> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            _route.IdentityConfiguration.Builder = builder;
            return this;
        }


        public IOnRouteEntryBuilder EnableEntityStorage(bool ignoreexceptions=true)
        {
            _route.StorageConfiguration.Enabled = true;
            _route.StorageConfiguration.IgnoreExceptions = ignoreexceptions;
            return this;
        }

        public IOnRouteEntryBuilder DisableEntityStorage()
        {
            _route.StorageConfiguration.Enabled = false;
            _route.StorageConfiguration.IgnoreExceptions = true;
            return this;
        }
    }
}