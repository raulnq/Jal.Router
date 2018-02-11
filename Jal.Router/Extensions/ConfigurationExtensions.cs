using System.Collections.Generic;
using System.Linq;
using Jal.Router.Model;

namespace Jal.Router.Extensions
{
    public static class MessageContextExtensions
    {
        public static void SendWithSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id=null, Dictionary<string,string> headers = null)
        {
            context.Send(content, context.CreateOriginForSaga(), context.CreateOptionsForSaga(endpointname, id, headers));
        }
        public static void SendWithSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Send(data, content, context.CreateOriginForSaga(), context.CreateOptionsForSaga(endpointname, id, headers));
        }

        public static void SendWithParentSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            if (context.Origin.ParentKeys.Count > 0 && context.SagaInfo.ParentIds.Count > 0 && context.ParentIds.Count>0)
            {
                context.Send(content, context.CreateOriginForParentSaga(), context.CreateOptionsForParentSaga(endpointname, id, headers));
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
                context.Send(data, content, context.CreateOriginForParentSaga(), context.CreateOptionsForParentSaga(endpointname, id, headers));
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
        public static void Send<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Send(data, content, context.CreateOptions(endpointname, id, headers));
        }

        public static void PublishWithSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(content, context.CreateOriginForSaga(), context.CreateOptionsForSaga(endpointname,  id, headers));
        }
        public static void PublishWithSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(data, content, context.CreateOriginForSaga(), context.CreateOptionsForSaga(endpointname, id, headers));
        }
        public static void Publish<TContent>(this MessageContext context, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(content, context.CreateOptions(endpointname, id, headers));
        }
        public static void Publish<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(data, content, context.CreateOptions(endpointname, id, headers));
        }

        public static void PublishToOrigin<TContent>(this MessageContext context, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(content, context.CreateOriginToOrigin(), context.CreateOptions(endpointname, id, headers));
        }
        public static void PublishToOrigin<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(data, content, context.CreateOriginToOrigin(), context.CreateOptions(endpointname, id, headers));
        }
        public static void PublishToOriginWithSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(content,context.CreateOriginToOriginForSaga(), context.CreateOptionsForSaga(endpointname, id, headers));
        }
        public static void PublishToOriginWithSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            context.Publish(data, content, context.CreateOriginToOriginForSaga(), context.CreateOptionsForSaga(endpointname, id, headers));
        }

        public static void PublishWithParentSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            if (context.Origin.ParentKeys.Count > 0 && context.SagaInfo.ParentIds.Count > 0 && context.ParentIds.Count > 0)
            {
                context.Publish(content, context.CreateOriginForParentSaga() , context.CreateOptionsForParentSaga(endpointname,  id, headers));
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
                context.Publish(data, content, context.CreateOriginForParentSaga(), context.CreateOptionsForParentSaga(endpointname, id, headers));
            }
            else
            {
                context.PublishWithSagaInfo(data, content, endpointname, id, headers);
            }
        }

        public static void PublishToOriginWithParentSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            if (context.Origin.ParentKeys.Count > 0 && context.SagaInfo.ParentIds.Count > 0 && context.ParentIds.Count > 0)
            {
                context.Publish(content, context.CreateOriginToOriginForParentSaga(), context.CreateOptionsForParentSaga(endpointname, id, headers));
            }
            else
            {
                context.PublishToOriginWithSagaInfo(content, endpointname, id, headers);
            }
        }
        public static void PublishToOriginWithParentSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null, Dictionary<string, string> headers = null)
        {
            if (context.Origin.ParentKeys.Count > 0 && context.SagaInfo.ParentIds.Count > 0 && context.ParentIds.Count > 0)
            {
                context.Publish(data, content, context.CreateOriginToOriginForParentSaga(), context.CreateOptionsForParentSaga(endpointname, id, headers));
            }
            else
            {
                context.PublishToOriginWithSagaInfo(data, content, endpointname, id, headers);
            }
        }

        public static Origin CreateOrigin(this MessageContext context)
        {
            return new Origin() { ParentKeys = context.Origin.ParentKeys };
        }

        public static Origin CreateOriginToOrigin(this MessageContext context)
        {
            return new Origin() { Key = context.Origin.Key, ParentKeys = context.Origin.ParentKeys };
        }

        public static Origin CreateOriginForSaga(this MessageContext context)
        {
            return new Origin() { ParentKeys = context.Origin.ParentKeys };
        }

        public static Origin CreateOriginToOriginForSaga(this MessageContext context)
        {
            return new Origin() { Key = context.Origin.Key, ParentKeys = context.Origin.ParentKeys };
        }

        public static Origin CreateOriginForParentSaga(this MessageContext context)
        {
            return new Origin() { ParentKeys = context.Origin.ParentKeys.Take(context.Origin.ParentKeys.Count - 1).ToList() };
        }

        public static Origin CreateOriginToOriginForParentSaga(this MessageContext context)
        {
            var newparentkeys = context.Origin.ParentKeys.Take(context.Origin.ParentKeys.Count - 1).ToList();

            var newparentkey = newparentkeys.LastOrDefault();

            return new Origin() { Key =  newparentkey ,ParentKeys = newparentkeys };
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
            var sagaparentid = context.SagaInfo.ParentIds.Last();

            var newsagaparentids = context.SagaInfo.ParentIds.Take(context.SagaInfo.ParentIds.Count - 1).ToList();

            var newparentids = context.ParentIds.Take(context.ParentIds.Count - 1).ToList();

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