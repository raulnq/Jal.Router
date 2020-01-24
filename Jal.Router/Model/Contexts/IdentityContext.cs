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

        public IdentityContext(string id, string operationid=null, string parentid=null, string partitionid=null, string replytorequestid=null, string requestid=null)
        {
            ReplyToRequestId = replytorequestid;
            RequestId = requestid;
            PartitionId = partitionid;
            ParentId = parentid;
            OperationId = operationid;
            Id = id;
        }

        public IdentityContext Clone()
        {
            return new IdentityContext(Id, OperationId, ParentId, PartitionId, ReplyToRequestId, RequestId);
        }

        public void SetParentId(string parentid)
        {
            ParentId = parentid;
        }

        public void SetOperationId(string operationid)
        {
            OperationId = operationid;
        }

        public void SetId(string id)
        {
            Id = id;
        }

        public void SetRequestId(string requestid)
        {
            RequestId = requestid;
        }

        public void SetReplyToRequestId(string replytorequestid)
        {
            ReplyToRequestId = replytorequestid;
        }

        public IdentityContextEntity ToEntity()
        {
            return new IdentityContextEntity(Id, OperationId, ParentId, RequestId, PartitionId, ReplyToRequestId);
        }
    }
}