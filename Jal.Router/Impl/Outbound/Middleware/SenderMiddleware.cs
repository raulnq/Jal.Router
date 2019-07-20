using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class SenderMiddleware : AbstractOutboundMessageHandler, IMiddlewareAsync<MessageContext>
    {
        private readonly ISender _sender;

        public SenderMiddleware(ISender sender, IComponentFactoryGateway factory, IConfiguration configuration) : base(configuration, factory)
        {
            _sender = sender;
        }

        public async Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            
            try
            {
                await _sender.Send(context.Data).ConfigureAwait(false);
            }
            finally
            {
                await CreateMessageEntityAndSave(context.Data).ConfigureAwait(false);
            }
            
        }
    }
}