namespace Jal.Router.Model
{
    public class Identity
    {
        public string Id { get; set; }
        public string OperationId { get; set; }
        public string ParentId { get; set; }
        public string ReplyToRequestId { get; set; }
        public string RequestId { get; set; }

        public Identity()
        {
            RequestId = string.Empty;
            ReplyToRequestId = string.Empty;
        }
    }
}