using System;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRouteEntryBuilder
    {
        IOnRouteEntryBuilder BuildOperationIdWith(Func<MessageContext, string> builder);

        IOnRouteEntryBuilder BuildIdWith(Func<MessageContext, string> builder);

        IOnRouteEntryBuilder BuildParentIdWith(Func<MessageContext, string> builder);

        IOnRouteEntryBuilder IgnoreExceptionOnSaveMessage(bool ignore);

        IOnRouteEntryBuilder SaveMessage(bool save);
    }
}