using System;

namespace Jal.Router.Model
{
    public class ContentContext
    {
        public Type Type { get; private set; }

        public string Data { get; private set; }

        public string Id { get; private set; }

        public MessageContext Context { get; private set; }

        public bool IsClaimCheck { get; private set; }

        public object Response { get; private set; }

        public ContentContext(MessageContext context, string id, bool isClaimCheck)
        {
            Id = id;
            Context = context;
            IsClaimCheck = isClaimCheck;
        }

        public ContentContext(MessageContext context, string id, bool isClaimCheck, Type type, string data):this(context, id, isClaimCheck)
        {
            Data = data;
            Type = type;
        }

        public void UpdateResponse(object response)
        {
            Response = response;
        }

        public void GenerateId()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ContentContextEntity ToEntity()
        {
            return new ContentContextEntity(Type, Data, Id);
        }
    }
}