namespace Jal.Router.Model.Inbound
{
    public class IndboundMessageContext<TContent> : MessageContext
    {
        public TContent Content { get; set; }

        public IndboundMessageContext(MessageContext context, TContent content)
        {
            Id = context.Id;
            Headers = context.Headers;
            Version = context.Version;
            RetryCount = context.RetryCount;
            LastRetry = context.LastRetry;
            Origin = context.Origin;
            DateTimeUtc = context.DateTimeUtc;
            Content = content;
            Saga = context.Saga;

        }
    }
}
