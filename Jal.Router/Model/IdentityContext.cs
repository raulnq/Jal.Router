﻿namespace Jal.Router.Model
{
    public class IdentityContext
    {
        public string Id { get; }
        public string OperationId { get; }
        public string ParentId { get; }
        public string ReplyToRequestId { get; set; }
        public string RequestId { get; set; }
        public string GroupId { get; }

        public  IdentityContext()
        {

        }

        public IdentityContext(string id)
        {
            Id = id;
        }

        public IdentityContext(string id, string operationid):this(id)
        {
            OperationId = operationid;
        }

        public IdentityContext(string id, string operationid, string parentid) :this(id, operationid)
        {
            ParentId = parentid;
        }

        public IdentityContext(string id, string operationid, string parentid, string groupid):this(id, operationid, parentid)
        {
            GroupId = groupid;
        }

        public IdentityContext Clone()
        {
            return new IdentityContext(Id, OperationId, ParentId, GroupId)
            {
                RequestId = RequestId,
                ReplyToRequestId = ReplyToRequestId
            };
        }

        public IdentityContextEntity ToEntity()
        {
            return new IdentityContextEntity(Id, OperationId, ParentId, RequestId, GroupId, ReplyToRequestId);
        }
    }
}