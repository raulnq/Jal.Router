using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRouteEntryBuilder
    {
        IOnRouteEntryBuilder BuildIdentityWith(Func<MessageContext, IdentityContext> builder);

        IOnRouteEntryBuilder EnableEntityStorage(bool ignoreexceptions=true);
        IOnRouteEntryBuilder DisableEntityStorage();
    }
}