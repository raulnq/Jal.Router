using System;
using System.Configuration;
using System.Security;
using System.Text;
using Jal.Router.Interface.Inbound;
using Jal.Router.Model;
using Jal.Router.Model.Inbound;

namespace Jal.Router.Impl.Inbound
{
    public class AppSettingsBasicAuthenticationHandler : IMiddleware
    {
        public void Execute<TContent>(MessageContext<TContent> context, Action next, MiddlewareParameter parameter)
        {
            if (!IsValid(context))
            {
                Console.WriteLine("Unauthorized");
                throw new SecurityException("Unauthorized");
            }
            next();
        }

        private bool IsValid(MessageContext context)
        {
            var key = ConfigurationManager.AppSettings["basicauthentication.key"];

            if (string.IsNullOrEmpty(key))
                return false;

            var secret = ConfigurationManager.AppSettings["basicauthentication.secret"];

            if (string.IsNullOrEmpty(secret))
                return false;

            if (!context.Headers.ContainsKey("Authorization"))
            {
                return false;
            }

            var header = context.Headers["Authorization"];

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

            return keyfromheader.ToLower() == key.ToLower() && secretfromheader == secret;
        }
    }
}