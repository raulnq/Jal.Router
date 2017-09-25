using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Jal.Router.Interface;
using Jal.Router.Model;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace Jal.Router.AzureServiceBus.Impl
{
    internal static class StrictEncodings
    {
        public static UTF8Encoding Utf8 { get; } = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
    }

    public class BrokeredMessageAdapter : IMessageAdapter<BrokeredMessage>
    {
        public string ReadBody(BrokeredMessage input)
        {
            using (var stream = input.GetBody<Stream>())
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

                    var clonedMessage = input.Clone();
                    try
                    {
                        return clonedMessage.GetBody<string>();
                    }
                    catch (Exception exception)
                    {
                        var contentType = input.ContentType ?? "null";

                        var msg = string.Format(CultureInfo.InvariantCulture, "The BrokeredMessage with ContentType '{0}' failed to deserialize to a string with the message: '{1}'", contentType, exception.Message);

                        throw new InvalidOperationException(msg, exception);
                    }
                    
                }
            }
        }

        public TContent ReadContent<TContent>(BrokeredMessage message)
        {
            var content = ReadBody(message);

            return JsonConvert.DeserializeObject<TContent>(content);
        }

        public InboundMessageContext ReadContext(BrokeredMessage message)
        {

            var context = new InboundMessageContext
            {
                Id = message.MessageId,
                DateTimeUtc = DateTime.UtcNow
            };

            if (message.Properties.ContainsKey("from"))
            {
                context.Origin.Name = message.Properties["from"].ToString();
            }

            if (message.Properties.ContainsKey("sagaid"))
            {
                context.Saga.Id = message.Properties["sagaid"].ToString();
            }

            if (message.Properties.ContainsKey("version"))
            {
                context.Version = message.Properties["version"].ToString();
            }

            if (message.Properties.ContainsKey("origin"))
            {
                context.Origin.Key = message.Properties["origin"].ToString();
            }

            if (message.Properties.ContainsKey("retrycount"))
            {
                context.RetryCount = Convert.ToInt32(message.Properties["retrycount"].ToString());
            }

            if (message.Properties != null)
            {
                foreach (var property in message.Properties.Where(x=>x.Key!= "from" && x.Key != "origin" && x.Key != "version" && x.Key != "retrycount"))
                {
                    context.Headers.Add(property.Key, property.Value?.ToString());
                }
            }

            return context;
        }

        public BrokeredMessage Write<TContent>(OutboundMessageContext<TContent> message)
        {
            var json = JsonConvert.SerializeObject(message.Content);

            var body = new MemoryStream(Encoding.UTF8.GetBytes(json)); 

            var brokeredmessage = new BrokeredMessage(body, true) { ContentType = "application/json" };

            foreach (var header in message.Headers)
            {
                brokeredmessage.Properties.Add(header.Key, header.Value);
            }

            if (!string.IsNullOrWhiteSpace(message.Origin.Name))
            {
                brokeredmessage.Properties.Add("from", message.Origin.Name);
            }

            if (!string.IsNullOrWhiteSpace(message.Version))
            {
                brokeredmessage.Properties.Add("version", message.Version);
            }

            if (!string.IsNullOrWhiteSpace(message.Saga.Id))
            {
                brokeredmessage.Properties.Add("sagaid", message.Saga.Id);
            }

            if (message.ScheduledEnqueueDateTimeUtc!=null)
            {
                brokeredmessage.ScheduledEnqueueTimeUtc = message.ScheduledEnqueueDateTimeUtc.Value;
            }

            if (!string.IsNullOrWhiteSpace(message.Origin.Key))
            {
                brokeredmessage.Properties.Add("origin", message.Origin.Key);
            }

            if (!string.IsNullOrWhiteSpace(message.Id))
            {
                brokeredmessage.MessageId = message.Id;
            }

            brokeredmessage.Properties.Add("retrycount", message.RetryCount.ToString());

            return brokeredmessage;
        }
    }
}