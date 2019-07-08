namespace Jal.Router.Model
{
    public class IdentityContextEntity
    {
        public string Id { get; }
        public string OperationId { get; }
        public string ParentId { get; }
        public string ReplyToRequestId { get; }
        public string RequestId { get; }
        public string GroupId { get; }

        public IdentityContextEntity()
        {

        }

        public IdentityContextEntity(string id, string operationid, string parentid, string requestid, string groupid, string replytorequestid)
        {
            Id = id;
            OperationId = operationid;
            ParentId = parentid;
            RequestId = requestid;
            GroupId = groupid;
            ReplyToRequestId = replytorequestid;
        }
    }
}
