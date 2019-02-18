using System;
using System.Collections.Generic;
using Jal.Router.Model;

namespace Jal.Router.Extensions
{
    public static class MessageContextExtensions
    {
        public static TResult Reply<TContent, TResult>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, Dictionary<string, string> headers = null)
        {
            var options = context.CreateOptions(endpointname, identitycontext, headers);

            options.Identity.ReplyToRequestId = Guid.NewGuid().ToString();

            return context.Reply<TContent, TResult>(content, options);
        }

        public static TResult Reply<TContent, TResult>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, string sagaid, Dictionary<string, string> headers = null)
        {
            var options = context.CreateOptions(endpointname, identitycontext, sagaid, headers);

            options.Identity.ReplyToRequestId = Guid.NewGuid().ToString();

            return context.Reply<TContent, TResult>(content, options);
        }

        public static void FireAndForget<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, string sagaid, Dictionary<string, string> headers = null)
        {
            context.FireAndForget(content, context.CreateOrigin(), context.CreateOptions(endpointname, identitycontext, sagaid, headers));
        }

        public static void FireAndForget<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext,  Dictionary<string, string> headers = null)
        {
            context.FireAndForget(content, context.CreateOrigin(), context.CreateOptions(endpointname, identitycontext, headers));
        }

        public static void Send<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, string sagaid, Dictionary<string, string> headers = null)
        {
            context.Send(content, context.CreateOrigin(), context.CreateOptions(endpointname, identitycontext, sagaid, headers));
        }

        public static void Send<TContent>(this MessageContext context, TContent content, EndPoint endpoint, IdentityContext identitycontext, string sagaid,  Dictionary<string,string> headers = null)
        {
            context.Send(content, endpoint, context.CreateOrigin(), context.CreateOptions(string.Empty, identitycontext, sagaid, headers));
        }
        public static void Send<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, IdentityContext identitycontext, string sagaid, Dictionary<string, string> headers = null)
        {
            context.Send(data, content, context.CreateOrigin(), context.CreateOptions(endpointname, identitycontext, sagaid, headers));
        }

        public static void Send<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, Dictionary<string, string> headers = null)
        {
            context.Send(content, context.CreateOptions(endpointname, identitycontext, headers));
        }

        public static void Send<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, IdentityContext identitycontext, Dictionary<string, string> headers = null)
        {
            context.Send(data, content, context.CreateOptions(endpointname, identitycontext, headers));
        }

        public static void Publish<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, string sagaid, string key, Dictionary<string, string> headers = null)
        {
            context.Publish(content, context.CreateOrigin(key), context.CreateOptions(endpointname, identitycontext, sagaid, headers));
        }

        public static void Publish<TContent>(this MessageContext context, TContent content, EndPoint endpoint, IdentityContext identitycontext, string sagaid, string key, Dictionary<string, string> headers = null)
        {
            context.Publish(content, endpoint, context.CreateOrigin(key), context.CreateOptions(string.Empty, identitycontext, sagaid, headers));
        }

        public static void Publish<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, IdentityContext identitycontext, string sagaid, string key, Dictionary<string, string> headers = null)
        {
            context.Publish(data, content, context.CreateOrigin(key), context.CreateOptions(endpointname, identitycontext, sagaid, headers));
        }
        public static void Publish<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, string key, Dictionary<string, string> headers = null)
        {
            context.Publish(content, context.CreateOrigin(key), context.CreateOptions(endpointname, identitycontext, headers));
        }

        public static void Publish<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, IdentityContext identitycontext, string key, Dictionary<string, string> headers = null)
        {
            context.Publish(data, content, context.CreateOrigin(key), context.CreateOptions(endpointname, identitycontext, headers));
        }

        public static Origin CreateOrigin(this MessageContext context, string key = null)
        {
            return new Origin() { Key = key};
        }

        public static Options CreateOptions(this MessageContext context, string endpointname, IdentityContext identitycontext, Dictionary<string, string> headers = null)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }
            if (string.IsNullOrWhiteSpace(identitycontext.Id))
            {
                throw new ArgumentNullException(identitycontext.Id);
            }
            var options = new Options()
            {
                EndPointName = endpointname,
                Headers = context.CopyHeaders(),
                Tracks = context.Tracks,
            };

            options.Identity.Id = identitycontext.Id;

            options.Identity.GroupId = identitycontext.GroupId;

            options.Identity.OperationId = identitycontext.OperationId;

            options.Identity.ParentId = context.IdentityContext.Id;

            if (!string.IsNullOrWhiteSpace(context.IdentityContext.ReplyToRequestId))
            {
                options.Identity.RequestId = context.IdentityContext.ReplyToRequestId;
                options.Identity.ReplyToRequestId = context.IdentityContext.ReplyToRequestId;
            }

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

            options.SagaContext.Id = sagaid;

            return options;
        }
    }
}