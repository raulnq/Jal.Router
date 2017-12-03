namespace Jal.Router.Model.Inbound
{
    public class InboundMessageContext<TContent> : MessageContext
    {
        public TContent Content { get; set; }

        public InboundMessageContext(MessageContext context, TContent content)
        {
            Id = context.Id;
            Headers = context.Headers;
            Version = context.Version;
            RetryCount = context.RetryCount;
            LastRetry = context.LastRetry;
            Origin = context.Origin;
            DateTimeUtc = context.DateTimeUtc;
            Content = content;
            SagaInfo = context.SagaInfo;
            Route = context.Route;
            ContentType = context.ContentType;
            Body = context.Body;
        }
    }
}
