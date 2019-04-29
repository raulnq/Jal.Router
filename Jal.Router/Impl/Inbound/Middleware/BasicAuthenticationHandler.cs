using System;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Jal.ChainOfResponsability.Intefaces;
using Jal.ChainOfResponsability.Model;
using Jal.Router.Model;

namespace Jal.Router.Impl.Inbound.Middleware
{
    public class BasicAuthenticationHandler : IMiddlewareAsync<MessageContext>
    {
        private readonly string _key;

        private readonly string _secret;

        public BasicAuthenticationHandler(string key, string secret)
        {
            _key = key;
            _secret = secret;
        }

        public Task ExecuteAsync(Context<MessageContext> context, Func<Context<MessageContext>, Task> next)
        {
            if (!IsValid(context.Data))
            {
                throw new SecurityException("Unauthorized");
            }
            return next(context);
        }

        private bool IsValid(MessageContext context)
        {
            if (string.IsNullOrEmpty(_key))
                return false;

            if (string.IsNullOrEmpty(_secret))
                return false;

            if (!context.Headers.ContainsKey("authorization"))
            {
                return false;
            }

            var header = context.Headers["authorization"];

            if (string.IsNullOrEmpty(header))
                return false;

            header = header.Trim();
            if (header.IndexOf("Basic ", StringComparison.InvariantCultureIgnoreCase) != 0)
                return false;

            var credentials = header.Substring(6);

            var credentialsBase64DecodedArray = Convert.FromBase64String(credentials);
            var decodedCredentials = Encoding.UTF8.GetString(credentialsBase64DecodedArray, 0, credentialsBase64DecodedArray.Length);

            var separatorPosition = decodedCredentials.IndexOf(':');

            if (separatorPosition <= 0)
                return false;

            var keyfromheader = decodedCredentials.Substring(0, separatorPosition);
            var secretfromheader = decodedCredentials.Substring(separatorPosition + 1);

            if (string.IsNullOrEmpty(keyfromheader) || string.IsNullOrEmpty(secretfromheader))
                return false;

            return keyfromheader.ToLower() == _key.ToLower() && secretfromheader == _secret;
        }
    }
}