namespace Jal.Router.Model
{
    public class TracingContext
    {
        public string Id { get; private set; }
        public string OperationId { get; private set; }
        public string ParentId { get; private set; }
        public string ReplyToRequestId { get; private set; }
        public string RequestId { get; private set;  }
        public string PartitionId { get; private set; }
        private  TracingContext()
        {

        }

        public TracingContext(string id, string operationid=null, string parentid=null, string partitionid=null, string replytorequestid=null, string requestid=null)
        {
            ReplyToRequestId = replytorequestid;
            RequestId = requestid;
            PartitionId = partitionid;
            ParentId = parentid;
            OperationId = operationid;
            Id = id;
        }

        public TracingContext CreateDependency(string newid, string newreplytorequestid)
        {
            var id = Id;

            if (!string.IsNullOrWhiteSpace(newid))
            {
                id = newid;
            }

            var replytorequestid = ReplyToRequestId;

            if (!string.IsNullOrWhiteSpace(newreplytorequestid))
            {
                replytorequestid = newreplytorequestid;
            }

            var requesid = RequestId;

            if (!string.IsNullOrWhiteSpace(replytorequestid))
            {
                requesid = replytorequestid;
            }

            var parentid = Id;

            return new TracingContext(id, OperationId, parentid, PartitionId, replytorequestid, requesid);
        }

        public TracingContextEntity ToEntity()
        {
            return new TracingContextEntity(Id, OperationId, ParentId, RequestId, PartitionId, ReplyToRequestId);
        }
    }
}