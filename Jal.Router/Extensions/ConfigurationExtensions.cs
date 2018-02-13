using System.Collections.Generic;
using System.Linq;
using Jal.Router.Model;

namespace Jal.Router.Extensions
{
    public static class MessageContextExtensions
    {
        public static void SendWithSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id=null, Dictionary<string,string> headers = null)
        {
            context.Send(content, context.CreateOrigin(), context.CreateOptionsForSaga(endpointname, id, headers));
        }
        public static void SendWithSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Send(data, content, context.CreateOrigin(), context.CreateOptionsForSaga(endpointname, id, headers));
        }

        public static void SendWithParentSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            if (context.Origin.ParentKeys.Count > 0 && context.SagaInfo.ParentIds.Count > 0 && context.ParentIds.Count>0)
            {
                context.Send(content, context.CreateOriginForParent(), context.CreateOptionsForParentSaga(endpointname, id, headers));
            }
            else
            {
                context.SendWithSagaInfo(content, endpointname, id, headers);
            }
        }

        public static void SendWithParentSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            if (context.Origin.ParentKeys.Count > 0 && context.SagaInfo.ParentIds.Count > 0 && context.ParentIds.Count > 0)
            {
                context.Send(data, content, context.CreateOriginForParent(), context.CreateOptionsForParentSaga(endpointname, id, headers));
            }
            else
            {
                context.SendWithSagaInfo(data, content, endpointname, id, headers);
            }
        }
        public static void Send<TContent>(this MessageContext context, TContent content, string endpointname, string id=null, Dictionary<string, string> headers = null)
        {
            context.Send(content, context.CreateOptions(endpointname, id, headers));
        }

        public static void Send<TContent>(this MessageContext context, TContent content, Origin origin, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Send(content, origin, context.CreateOptions(endpointname, id, headers));
        }

        public static void Send<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Send(data, content, context.CreateOptions(endpointname, id, headers));
        }

        public static void Send<TContent, TData>(this MessageContext context, TData data, TContent content, Origin origin, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Send(data, content, origin, context.CreateOptions(endpointname, id, headers));
        }

        public static void PublishWithSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(content, context.CreateOrigin(), context.CreateOptionsForSaga(endpointname,  id, headers));
        }
        public static void PublishWithSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(data, content, context.CreateOrigin(), context.CreateOptionsForSaga(endpointname, id, headers));
        }
        public static void Publish<TContent>(this MessageContext context, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(content, context.CreateOptions(endpointname, id, headers));
        }
        public static void Publish<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(data, content, context.CreateOptions(endpointname, id, headers));
        }

        public static void Publish<TContent>(this MessageContext context, TContent content, string endpointname, Origin origin, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(content, origin, context.CreateOptions(endpointname, id, headers));
        }
        public static void Publish<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, Origin origin, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(data, content, origin, context.CreateOptions(endpointname, id, headers));
        }
        public static void PublishWithSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, Origin origin, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(content, origin, context.CreateOptionsForSaga(endpointname, id, headers));
        }
        public static void PublishWithSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, Origin origin, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(data, content, origin, context.CreateOptionsForSaga(endpointname, id, headers));
        }

        public static void PublishWithParentSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            if (context.Origin.ParentKeys.Count > 0 && context.SagaInfo.ParentIds.Count > 0 && context.ParentIds.Count > 0)
            {
                context.Publish(content, context.CreateOriginForParent() , context.CreateOptionsForParentSaga(endpointname,  id, headers));
            }
            else
            {
                context.PublishWithSagaInfo(content, endpointname, id, headers);
            }
        }
        public static void PublishWithParentSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            if (context.Origin.ParentKeys.Count > 0 && context.SagaInfo.ParentIds.Count > 0 && context.ParentIds.Count > 0)
            {
                context.Publish(data, content, context.CreateOriginForParent(), context.CreateOptionsForParentSaga(endpointname, id, headers));
            }
            else
            {
                context.PublishWithSagaInfo(data, content, endpointname, id, headers);
            }
        }

        public static void PublishWithParentSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, Origin origin, string id = null, Dictionary<string, string> headers = null)
        {
            if (context.SagaInfo.ParentIds.Count > 0 && context.ParentIds.Count > 0)
            {
                context.Publish(content, origin, context.CreateOptionsForParentSaga(endpointname, id, headers));
            }
            else
            {
                context.PublishWithSagaInfo(content, endpointname, origin, id, headers);
            }
        }
        public static void PublishWithParentSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, Origin origin, string id = null, Dictionary<string, string> headers = null)
        {
            if (context.SagaInfo.ParentIds.Count > 0 && context.ParentIds.Count > 0)
            {
                context.Publish(data, content, origin, context.CreateOptionsForParentSaga(endpointname, id, headers));
            }
            else
            {
                context.PublishWithSagaInfo(data, content, endpointname, origin, id, headers);
            }
        }

        public static Origin CreateOrigin(this MessageContext context, string key = null)
        {
            return new Origin() { ParentKeys = context.Origin.ParentKeys, Key = key};
        }

        public static Origin CreateOriginForParent(this MessageContext context, string key = null)
        {
            return new Origin() { ParentKeys = context.Origin.ParentKeys.Take(context.Origin.ParentKeys.Count - 1).ToList(), Key = key};
        }


        public static Options CreateOptions(this MessageContext context, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            var options = new Options()
            {
                EndPointName = endpointname,
                Headers = context.CopyHeaders(),
                Id = string.IsNullOrWhiteSpace(id) ? context.Id : id,
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    options.Headers.Add(header);
                }
            }

            return options;
        }

        public static Options CreateOptionsForSaga(this MessageContext context, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            var options = new Options()
            {
                EndPointName = endpointname,
                Headers = context.CopyHeaders(),
                Id = string.IsNullOrWhiteSpace(id) ? context.Id : id,
                ParentIds = context.ParentIds,
                SagaInfo = new SagaInfo() { Id = context.SagaInfo.Id, ParentIds = context.SagaInfo.ParentIds}
            };

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    options.Headers.Add(header);
                }
            }

            return options;
        }

        public static Options CreateOptionsForParentSaga(this MessageContext context, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            
            var newsagaparentids = context.SagaInfo.ParentIds.Take(context.SagaInfo.ParentIds.Count - 1).ToList();

            var newparentids = context.ParentIds.Take(context.ParentIds.Count - 1).ToList();

            var sagaparentid = newsagaparentids.LastOrDefault();

            var parentid = newparentids.LastOrDefault();

            var options = new Options()
            {
                EndPointName = endpointname,
                Headers = context.CopyHeaders(),
                Id = string.IsNullOrWhiteSpace(id) ? (string.IsNullOrWhiteSpace(parentid) ? context.Id : parentid) : id,
                ParentIds = newparentids,
                SagaInfo = context.SagaInfo.ParentIds.Count>0 ? new SagaInfo() {Id = sagaparentid, ParentIds = newsagaparentids} : new SagaInfo() { Id = context.SagaInfo.Id }
            };

            if (headers == null) return options;

            foreach (var header in headers)
            {
                options.Headers.Add(header);
            }


            return options;
        }
    }
}