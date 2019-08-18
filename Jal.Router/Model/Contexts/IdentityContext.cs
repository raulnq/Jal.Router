namespace Jal.Router.Model
{
    public class IdentityContext
    {
        public string Id { get; private set; }
        public string OperationId { get; private set; }
        public string ParentId { get; private set; }
        public string ReplyToRequestId { get; private set; }
        public string RequestId { get; private set;  }
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

        public IdentityContext(string id, string operationid, string parentid, string partitionid, string replytorequestid, string requestid) : this(id, operationid, parentid, partitionid)
        {
            ReplyToRequestId = replytorequestid;
            RequestId = requestid;
        }

        public IdentityContext CreateCopy()
        {
            return new IdentityContext(Id, OperationId, ParentId, PartitionId)
            {
                RequestId = RequestId,
                ReplyToRequestId = ReplyToRequestId
            };
        }

        public void UpdateParentId(string parentid)
        {
            ParentId = parentid;
        }

        public void UpdateOperationId(string operationid)
        {
            OperationId = operationid;
        }

        public void UpdateId(string id)
        {
            Id = id;
        }

        public void UpdateRequestId(string requestid)
        {
            RequestId = requestid;
        }

        public void UpdateReplyToRequestId(string replytorequestid)
        {
            ReplyToRequestId = replytorequestid;
        }

        public IdentityContextEntity ToEntity()
        {
            return new IdentityContextEntity(Id, OperationId, ParentId, RequestId, PartitionId, ReplyToRequestId);
        }
    }
}