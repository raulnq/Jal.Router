using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class EndPoint
    {
        public EndPoint(string name)
        {
            Name = name;
            Channels = new List<Channel>();
            MiddlewareTypes = new List<Type>();
            ErrorHandlers = new List<ErrorHandler>();
            EntryHandlers = new List<Handler>();
            ExitHandlers = new List<Handler>();
            Condition = null;
        }
        public Origin Origin { get; private set; }

        public string Name { get; private set; }

        public Func<Options, object, bool> Condition { get; private set; }

        public Type ReplyContentType { get; private set; }

        public IList<Channel> Channels { get; }

        public IList<Type> MiddlewareTypes { get; }

        public bool UseClaimCheck { get; private set; }

        public IList<ErrorHandler> ErrorHandlers { get; }

        public IList<Handler> EntryHandlers { get; }

        public IList<Handler> ExitHandlers { get; }

        public override string ToString()
        {
            return Name;
        }
        public EndpointEntity ToEntity()
        {
            return new EndpointEntity(Name);
        }

        public void SetOrigin(Origin origin)
        {
            Origin = origin;
        }

        public void When(Func<Options, object, bool> condition)
        {
            Condition = condition;
        }

        public void SetReplyContentType(Type contenttype)
        {
            ReplyContentType = contenttype;
        }

        public void AsClaimCheck()
        {
            UseClaimCheck = true;
        }
    }
}