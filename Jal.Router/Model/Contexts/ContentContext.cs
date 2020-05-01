﻿using System;

namespace Jal.Router.Model
{
    public class ContentContext
    {
        public string Data { get; private set; }

        public string ClaimCheckId { get; private set; }

        public MessageContext Context { get; private set; }

        public bool UseClaimCheck { get; private set; }

        public object Result { get; private set; }

        public ContentContext(MessageContext context, string claimcheckid, bool useclaimcheck, string data)
        {
            Data = data;
            ClaimCheckId = claimcheckid;
            Context = context;
            UseClaimCheck = useclaimcheck;
        }

        public void SetResult(object result)
        {
            Result = result;
        }

        public ContentContextEntity ToEntity()
        {
            return new ContentContextEntity(Data, ClaimCheckId);
        }
    }
}