using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Extensions
{
    public static class MessageContextExtensions
    {
        public static Task<TResult> Reply<TContent, TResult>(this MessageContext context, TContent content, string endpointname, string id = null, string sagaid = null, Dictionary<string, string> headers = null, string version = null) where TResult : class
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            var options = context.CreateOptions(endpointname, id, sagaid, headers);

            options.IdentityContext.UpdateReplyToRequestId(Guid.NewGuid().ToString());

            return context.Reply<TContent, TResult>(content, options);
        }

        public static Task<TResult> Reply<TContent, TResult>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, string sagaid = null, Dictionary<string, string> headers = null, string version = null) where TResult : class
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            var options = context.CreateOptions(endpointname, identitycontext, sagaid, headers, version);

            options.IdentityContext.UpdateReplyToRequestId(Guid.NewGuid().ToString());

            return context.Reply<TContent, TResult>(content, options);
        }

        public static Task FireAndForget<TContent>(this MessageContext context, TContent content, EndPoint endpoint, IdentityContext identitycontext, string sagaid = null, Dictionary<string, string> headers = null)
        {
            return context.FireAndForget(content, endpoint, context.CreateOrigin(), context.CreateOptions(string.Empty, identitycontext, sagaid, headers));
        }

        public static Task FireAndForget<TContent>(this MessageContext context, TContent content, EndPoint endpoint, string id = null, string sagaid = null, Dictionary<string, string> headers = null, string version = null)
        {
            return context.FireAndForget(content, endpoint, context.CreateOrigin(), context.CreateOptions(string.Empty, id, sagaid, headers, version));
        }

        public static Task FireAndForget<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, string sagaid = null, Dictionary<string, string> headers = null)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            return context.FireAndForget(content, context.CreateOrigin(), context.CreateOptions(endpointname, identitycontext, sagaid, headers));
        }

        public static Task FireAndForget<TContent>(this MessageContext context, TContent content, string endpointname, string id = null, string sagaid=null,  Dictionary<string, string> headers = null, string version = null)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            return context.FireAndForget(content, context.CreateOrigin(), context.CreateOptions(endpointname, id, sagaid, headers, version));
        }

        public static Task Send<TContent>(this MessageContext context, TContent content, EndPoint endpoint, IdentityContext identitycontext, string sagaid = null, Dictionary<string,string> headers = null)
        {
            return context.Send(content, endpoint, context.CreateOrigin(), context.CreateOptions(string.Empty, identitycontext, sagaid, headers));
        }

        public static Task Send<TContent>(this MessageContext context, TContent content, EndPoint endpoint, string id=null, string sagaid = null, Dictionary<string, string> headers = null, string version = null)
        {
            return context.Send(content, endpoint, context.CreateOrigin(), context.CreateOptions(string.Empty, id, sagaid, headers, version));
        }

        public static Task Send<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, string sagaid = null, Dictionary<string, string> headers = null, string version = null)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            return context.Send(content, context.CreateOptions(endpointname, identitycontext, sagaid, headers, version));
        }

        public static Task Send<TContent>(this MessageContext context, TContent content, string endpointname, string id=null, string sagaid=null, Dictionary<string, string> headers = null, string version=null)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            return context.Send(content, context.CreateOptions(endpointname, id, sagaid, headers, version));
        }

        public static Task Publish<TContent>(this MessageContext context, TContent content, string endpointname, IdentityContext identitycontext, string sagaid=null, string key=null, Dictionary<string, string> headers = null, string version = null)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            return context.Publish(content, context.CreateOrigin(key), context.CreateOptions(endpointname, identitycontext, sagaid, headers, version));
        }

        public static Task Publish<TContent>(this MessageContext context, TContent content, EndPoint endpoint, IdentityContext identitycontext, string sagaid = null, string key = null, Dictionary<string, string> headers = null, string version = null)
        {
            return context.Publish(content, endpoint, context.CreateOrigin(key), context.CreateOptions(string.Empty, identitycontext, sagaid, headers, version));
        }

        public static Task Publish<TContent>(this MessageContext context, TContent content, string endpointname, string id = null, string sagaid = null, string key = null, Dictionary<string, string> headers = null, string version = null)
        {
            if (string.IsNullOrWhiteSpace(endpointname))
            {
                throw new ArgumentNullException(endpointname);
            }

            return context.Publish(content, context.CreateOrigin(key), context.CreateOptions(endpointname, id, sagaid, headers, version));
        }

        public static Task Publish<TContent>(this MessageContext context, TContent content, EndPoint endpoint, string id = null, string sagaid = null, string key = null, Dictionary<string, string> headers = null, string version = null)
        {
            return context.Publish(content, endpoint, context.CreateOrigin(key), context.CreateOptions(string.Empty, id, sagaid, headers, version));
        }

        public static Origin CreateOrigin(this MessageContext context, string key = null)
        {
            return new Origin() { Key = key};
        }

        private static Options CreateOptions(this MessageContext context, string endpointname, IDictionary<string, string> headers = null, string version = null)
        {
            var identitycontext = context.IdentityContext.CreateCopy();

            if (string.IsNullOrWhiteSpace(identitycontext.Id))
            {
                throw new ArgumentNullException(identitycontext.Id);
            }

            identitycontext.UpdateParentId(identitycontext.Id);

            if (!string.IsNullOrWhiteSpace(context.IdentityContext.ReplyToRequestId))
            {
                identitycontext.UpdateRequestId(context.IdentityContext.ReplyToRequestId);
            }

            if(string.IsNullOrEmpty(version))
            {
                version = context.Version;
            }

            var options = new Options(endpointname, context.CreateCopyOfHeaders(), context.SagaContext, context.TrackingContext, identitycontext, context.Route, context.Saga, version);

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

        private static Options CreateOptions(this MessageContext context, string endpointname, IdentityContext identitycontext, Dictionary<string, string> headers = null, string version = null)
        {

            if (string.IsNullOrWhiteSpace(identitycontext.Id))
            {
                throw new ArgumentNullException(identitycontext.Id);
            }

            if (!string.IsNullOrWhiteSpace(context.IdentityContext.ReplyToRequestId))
            {
                identitycontext.UpdateRequestId(context.IdentityContext.ReplyToRequestId);

                identitycontext.UpdateReplyToRequestId(context.IdentityContext.ReplyToRequestId);
            }

            if (string.IsNullOrEmpty(version))
            {
                version = context.Version;
            }

            var options = new Options(endpointname, context.CreateCopyOfHeaders(), context.SagaContext, context.TrackingContext, identitycontext, context.Route, context.Saga, version);

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

        public static Options CreateOptions(this MessageContext context, string endpointname, IdentityContext indentitycontext, string sagaid=null, Dictionary<string, string> headers = null, string version = null)
        {
            var options = CreateOptions(context, endpointname, indentitycontext, headers, version);

            if (!string.IsNullOrEmpty(sagaid))
            {
                options.SagaContext.SetId(sagaid);
            }

            return options;
        }

        public static Options CreateOptions(this MessageContext context, string endpointname, string id=null, string sagaid = null, Dictionary<string, string> headers = null, string version = null)
        {
            var options = CreateOptions(context, endpointname, headers, version);

            if(!string.IsNullOrEmpty(sagaid))
            {
                options.SagaContext.SetId(sagaid);
            }

            if (!string.IsNullOrEmpty(id))
            {
                options.IdentityContext.UpdateId(id);
            }

            return options;
        }
    }
}