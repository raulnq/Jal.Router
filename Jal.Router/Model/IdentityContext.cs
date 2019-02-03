namespace Jal.Router.Model
{
    public class IdentityContext
    {
        public string Id { get; set; }
        public string OperationId { get; set; }
        public string ParentId { get; set; }
        public string ReplyToRequestId { get; set; }
        public string RequestId { get; set; }
        public string GroupId { get; set; }
        public IdentityContext()
        {
            RequestId = string.Empty;
            ReplyToRequestId = string.Empty;
            GroupId = string.Empty;
        }
    }
}