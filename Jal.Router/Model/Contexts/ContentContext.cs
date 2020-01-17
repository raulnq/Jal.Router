﻿using System;

namespace Jal.Router.Model
{
    public class ContentContext
    {
        public Type Type { get; private set; }

        public string Data { get; private set; }

        public string ClaimCheckId { get; private set; }

        public MessageContext Context { get; private set; }

        public bool IsClaimCheck { get; private set; }

        public object Response { get; private set; }

        public ContentContext(MessageContext context, string claimcheckid, bool isclaimcheck)
        {
            ClaimCheckId = claimcheckid;
            Context = context;
            IsClaimCheck = isclaimcheck;
        }

        public ContentContext(MessageContext context, string claimcheckid, bool isclaimcheck, Type type, string data):this(context, claimcheckid, isclaimcheck)
        {
            Data = data;
            Type = type;
        }

        public void UpdateResponse(object response)
        {
            Response = response;
        }

        public void GenerateClaimCheckId()
        {
            ClaimCheckId = Guid.NewGuid().ToString();
        }

        public ContentContextEntity ToEntity()
        {
            return new ContentContextEntity(Type, Data, ClaimCheckId);
        }
    }
}