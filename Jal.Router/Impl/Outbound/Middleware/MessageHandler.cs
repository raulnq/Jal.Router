using System;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Outbound.Middleware
{
    public class MessageHandler : AbstractOutboundMessageHandler, IMiddlewareAsync<MessageContext>
    {
        private readonly ISender _sender;

        public MessageHandler(ISender sender, IComponentFactoryGateway factory, IConfiguration configuration) : base(configuration, factory)
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