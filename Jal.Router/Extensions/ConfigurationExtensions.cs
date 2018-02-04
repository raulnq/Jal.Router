using Jal.Router.Model;

namespace Jal.Router.Extensions
{
    public static class MessageContextExtensions
    {
        public static Options CreateOptions(this MessageContext context, string endpointname, string id = null)
        {
            return new Options()
            {
                EndPointName = endpointname,
                Headers = context.CopyHeaders(),
                Id = string.IsNullOrWhiteSpace(id) ? context.Id : id,
            };
        }

        public static Options CreateOptionsForSaga(this MessageContext context, string endpointname, string id = null)
        {
            return new Options()
            {
                EndPointName = endpointname,
                Headers = context.CopyHeaders(),
                Id = string.IsNullOrWhiteSpace(id) ? context.Id : id,
                SagaInfo = new SagaInfo() { Id = context.SagaInfo.Id }
            };
        }
    }
}