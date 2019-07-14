﻿namespace Jal.Router.Model
{
    public class IdentityContext
    {
        public string Id { get; private set; }
        public string OperationId { get; private set; }
        public string ParentId { get; private set; }
        public string ReplyToRequestId { get; set; }
        public string RequestId { get; set;  }
        public string PartitionId { get; private set; }
        private  IdentityContext()
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

        public IdentityContext(string id, string operationid, string parentid, string partitionid):this(id, operationid, parentid)
        {
            PartitionId = partitionid;
        }

        public IdentityContext Clone()
        {
            return new IdentityContext(Id, OperationId, ParentId, PartitionId)
            {
                RequestId = RequestId,
                ReplyToRequestId = ReplyToRequestId
            };
        }

        public IdentityContextEntity ToEntity()
        {
            return new IdentityContextEntity(Id, OperationId, ParentId, RequestId, PartitionId, ReplyToRequestId);
        }
    }
}