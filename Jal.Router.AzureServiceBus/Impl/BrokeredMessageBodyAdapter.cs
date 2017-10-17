using System;
using System.Globalization;
using System.IO;
using System.Text;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Interface.Management;
using Microsoft.ServiceBus.Messaging;

namespace Jal.Router.AzureServiceBus.Impl
{
    internal static class StrictEncodings
    {
        public static UTF8Encoding Utf8 { get; } = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
    }

    public class BrokeredMessageBodyAdapter : AbstractMessageBodyAdapter
    {
        public override string ReadBody<TMessage>(TMessage message)
        {
            var brokeredmesage = message as BrokeredMessage;

            if (brokeredmesage != null)
            { 
                using (var stream = brokeredmesage.GetBody<Stream>())
                {
                    if (stream == null)
                    {
                        return null;
                    }
                    using (TextReader reader = new StreamReader(stream, StrictEncodings.Utf8))
                    {

                        try
                        {
                            return reader.ReadToEnd();
                        }
                        catch (Exception)
                        {
                            // ignored
                        }

                        var clonedMessage = brokeredmesage.Clone();
                        try
                        {
                            return clonedMessage.GetBody<string>();
                        }
                        catch (Exception exception)
                        {
                            var contentType = brokeredmesage.ContentType ?? "null";

                            var msg = string.Format(CultureInfo.InvariantCulture, "The BrokeredMessage with ContentType '{0}' failed to deserialize to a string with the message: '{1}'", contentType, exception.Message);

                            throw new InvalidOperationException(msg, exception);
                        }
                    
                    }
                }
            }
            throw new ApplicationException($"Invalid message type {typeof(TMessage).FullName}");
        }

        public override TMessage WriteBody<TMessage>(string body)
        {
           
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(body));

            var brokeredmessage = new BrokeredMessage(stream, true) { ContentType = "application/json" };

            return (TMessage)Convert.ChangeType(brokeredmessage, typeof(TMessage));
        }

        public BrokeredMessageBodyAdapter(IComponentFactory factory, IConfiguration configuration) : base(factory, configuration)
        {
        }
    }
}