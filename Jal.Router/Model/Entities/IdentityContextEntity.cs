namespace Jal.Router.Model
{
    public class IdentityContextEntity
    {
        public string Id { get; private set; }
        public string OperationId { get; private set; }
        public string ParentId { get; private set; }
        public string ReplyToRequestId { get; private set; }
        public string RequestId { get; private set; }
        public string GroupId { get; private set; }

        private IdentityContextEntity()
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
