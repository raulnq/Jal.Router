using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Extensions
{
    public static class MessageContextExtensions
    {
        public static Task<TResult> Reply<TContent, TResult>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, Dictionary<string, string> headers = null)
        {
            var options = context.CreateOptions(endpointname, identitycontext, headers);

            options.IdentityContext.ReplyToRequestId = Guid.NewGuid().ToString();

            return context.Reply<TContent, TResult>(content, options);
        }

        public static Task<TResult> Reply<TContent, TResult>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, string sagaid, Dictionary<string, string> headers = null)
        {
            var options = context.CreateOptions(endpointname, identitycontext, sagaid, headers);

            options.IdentityContext.ReplyToRequestId = Guid.NewGuid().ToString();

            return context.Reply<TContent, TResult>(content, options);
        }

        public static Task FireAndForget<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, string sagaid, Dictionary<string, string> headers = null)
        {
            return context.FireAndForget(content, context.CreateOrigin(), context.CreateOptions(endpointname, identitycontext, sagaid, headers));
        }

        public static Task FireAndForget<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext,  Dictionary<string, string> headers = null)
        {
            return context.FireAndForget(content, context.CreateOrigin(), context.CreateOptions(endpointname, identitycontext, headers));
        }

        public static Task Send<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, string sagaid, Dictionary<string, string> headers = null)
        {
            return context.Send(content, context.CreateOrigin(), context.CreateOptions(endpointname, identitycontext, sagaid, headers));
        }

        public static Task Send<TContent>(this MessageContext context, TContent content, EndPoint endpoint, IdentityContext identitycontext, string sagaid,  Dictionary<string,string> headers = null)
        {
            return context.Send(content, endpoint, context.CreateOrigin(), context.CreateOptions(string.Empty, identitycontext, sagaid, headers));
        }

        public static Task Send<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, Dictionary<string, string> headers = null)
        {
            return context.Send(content, context.CreateOptions(endpointname, identitycontext, headers));
        }

        public static Task Publish<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, string sagaid, string key, Dictionary<string, string> headers = null)
        {
            return context.Publish(content, context.CreateOrigin(key), context.CreateOptions(endpointname, identitycontext, sagaid, headers));
        }

        public static Task Publish<TContent>(this MessageContext context, TContent content, EndPoint endpoint, IdentityContext identitycontext, string sagaid, string key, Dictionary<string, string> headers = null)
        {
            return context.Publish(content, endpoint, context.CreateOrigin(key), context.CreateOptions(string.Empty, identitycontext, sagaid, headers));
        }

        public static Task Publish<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, string key, Dictionary<string, string> headers = null)
        {
            return context.Publish(content, context.CreateOrigin(key), context.CreateOptions(endpointname, identitycontext, headers));
        }

        public static Origin CreateOrigin(this MessageContext context, string key = null)
        {
            return new Origin() { Key = key};
        }

        public static Options CreateOptions(this MessageContext context, string endpointname, IDictionary<string, string> headers = null, string version = "1")
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            var options = new Options(endpointname, context.CopyHeaders(), context.SagaContext, context.TrackingContext.Tracks, context.IdentityContext.Clone(), context.Route, version);

            if (!string.IsNullOrWhiteSpace(context.IdentityContext.ReplyToRequestId))
            {
                options.IdentityContext.RequestId = context.IdentityContext.ReplyToRequestId;

                options.IdentityContext.ReplyToRequestId = context.IdentityContext.ReplyToRequestId;
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if (headers.ContainsKey(header.Key))
                    {
                        options.Headers[header.Key] = header.Value;
                    }
                    else
                    {
                        options.Headers.Add(header);
                    }
                }
            }

            return options;
        }

        public static Options CreateOptions(this MessageContext context, string endpointname, IdentityContext identitycontext, Dictionary<string, string> headers = null, string version = "1")
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }
            if (string.IsNullOrWhiteSpace(identitycontext.Id))
            {
                throw new ArgumentNullException(identitycontext.Id);
            }

            if (!string.IsNullOrWhiteSpace(context.IdentityContext.ReplyToRequestId))
            {
                identitycontext.RequestId = context.IdentityContext.ReplyToRequestId;

                identitycontext.ReplyToRequestId = context.IdentityContext.ReplyToRequestId;
            }

            var options = new Options(endpointname, context.CopyHeaders(), context.SagaContext, context.TrackingContext.Tracks, identitycontext, context.Route, version);

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    if(headers.ContainsKey(header.Key))
                    {
                        options.Headers[header.Key]= header.Value;
                    }
                    else
                    {
                        options.Headers.Add(header);
                    }
                }
            }

            return options;
        }

        public static Options CreateOptions(this MessageContext context, string endpointname, IdentityContext indentitycontext, string sagaid, Dictionary<string, string> headers = null)
        {
            var options = CreateOptions(context, endpointname, indentitycontext, headers);

            options.SagaContext.UpdateId(sagaid);

            return options;
        }
    }
}