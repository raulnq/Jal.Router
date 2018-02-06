using System.Linq;
using Jal.Router.Model;

namespace Jal.Router.Extensions
{
    public static class MessageContextExtensions
    {
        public static void SendWithSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id=null)
        {
            context.Send(content, new Origin() {ParentKeys = context.Origin.ParentKeys}, context.CreateOptionsForSaga(endpointname, id));
        }
        public static void SendWithSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null)
        {
            context.Send(data, content, new Origin() { ParentKeys = context.Origin.ParentKeys }, context.CreateOptionsForSaga(endpointname, id));
        }

        public static void SendWithParentSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id = null)
        {
            if (context.Origin.ParentKeys.Count > 0)
            {
                context.Send(content, new Origin() { ParentKeys = context.Origin.ParentKeys.Take(context.Origin.ParentKeys.Count - 1).ToList() }, context.CreateOptionsForParentSaga(endpointname, id));
            }
            else
            {
                context.Send(content, context.CreateOptionsForParentSaga(endpointname, id));
            }
        }
        public static void SendWithParentSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null)
        {
            if (context.Origin.ParentKeys.Count > 0)
            {
                context.Send(data, content, new Origin() { ParentKeys = context.Origin.ParentKeys.Take(context.Origin.ParentKeys.Count - 1).ToList() }, context.CreateOptionsForParentSaga(endpointname, id));
            }
            else
            {
                context.Send(data, content, context.CreateOptionsForParentSaga(endpointname, id));
            }
        }
        public static void Send<TContent>(this MessageContext context, TContent content, string endpointname, string id=null)
        {
            context.Send(content, context.CreateOptions(endpointname, id));
        }
        public static void Send<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null)
        {
            context.Send(data, content, context.CreateOptions(endpointname, id));
        }



        public static void PublishWithSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id = null)
        {
            context.Publish(content, new Origin() { ParentKeys = context.Origin.ParentKeys },  context.CreateOptionsForSaga(endpointname, id));
        }
        public static void PublishWithSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null)
        {
            context.Publish(data, content, new Origin() { ParentKeys = context.Origin.ParentKeys }, context.CreateOptionsForSaga(endpointname, id));
        }
        public static void Publish<TContent>(this MessageContext context, TContent content, string endpointname, string id = null)
        {
            context.Publish(content, context.CreateOptions(endpointname, id));
        }
        public static void Publish<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null)
        {
            context.Publish(data, content, context.CreateOptions(endpointname, id));
        }
        public static void PublishToOrigin<TContent>(this MessageContext context, TContent content, string endpointname, string id = null)
        {
            context.Publish(content, new Origin() { Key = context.Origin.Key }, context.CreateOptions(endpointname, id));
        }
        public static void PublishToOrigin<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null)
        {
            context.Publish(data, content, new Origin() { Key = context.Origin.Key }, context.CreateOptions(endpointname, id));
        }
        public static void PublishToOriginWithSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id = null)
        {
            context.Publish(content, new Origin() { Key = context.Origin.Key, ParentKeys = context.Origin.ParentKeys }, context.CreateOptionsForSaga(endpointname, id));
        }
        public static void PublishToOriginWithSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null)
        {
            context.Publish(data, content, new Origin() { Key = context.Origin.Key, ParentKeys = context.Origin.ParentKeys }, context.CreateOptionsForSaga(endpointname, id));
        }


        public static void PublishWithParentSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id = null)
        {
            if (context.Origin.ParentKeys.Count > 0)
            {
                context.Publish(content, new Origin() { ParentKeys = context.Origin.ParentKeys.Take(context.Origin.ParentKeys.Count - 1).ToList() }, context.CreateOptionsForParentSaga(endpointname, id));
            }
            else
            {
                context.Publish(content, context.CreateOptionsForParentSaga(endpointname, id));
            }
        }
        public static void PublishWithParentSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null)
        {
            if (context.Origin.ParentKeys.Count > 0)
            {
                context.Publish(data, content, new Origin() { ParentKeys = context.Origin.ParentKeys.Take(context.Origin.ParentKeys.Count - 1).ToList() }, context.CreateOptionsForParentSaga(endpointname, id));
            }
            else
            {
                context.Publish(data, content, context.CreateOptionsForParentSaga(endpointname, id));
            }
        }

        public static void PublishToOriginWithParentSagaInfo<TContent>(this MessageContext context, TContent content, string endpointname, string id = null)
        {
            context.Publish(content, context.Origin.ParentKeys.Count > 0 ? new Origin()
            {
                Key = context.Origin.ParentKeys.Last(),
                ParentKeys = context.Origin.ParentKeys.Take(context.Origin.ParentKeys.Count - 1).ToList()
            }
            : new Origin() { Key = context.Origin.Key }, context.CreateOptionsForParentSaga(endpointname, id));
        }
        public static void PublishToOriginWithParentSagaInfo<TContent, TData>(this MessageContext context, TData data, TContent content, string endpointname, string id = null)
        {
            context.Publish(data, content, context.Origin.ParentKeys.Count > 0? new Origin()
            {
                Key = context.Origin.ParentKeys.Last(),
                ParentKeys = context.Origin.ParentKeys.Take(context.Origin.ParentKeys.Count - 1).ToList()
            }
            : new Origin() { Key = context.Origin.Key }, context.CreateOptionsForParentSaga(endpointname, id));
        }

        public static Options CreateOptions(this MessageContext context, string endpointname, string id = null)
        {
            return new Options()
            {
                EndPointName = endpointname,
                Headers = context.CopyHeaders(),
                Id = string.IsNullOrWhiteSpace(id) ? context.Id : id,
            };
        }

        public static Options CreateOptionsForSaga(this MessageContext context, string endpointname, string id = null)
        {
            return new Options()
            {
                EndPointName = endpointname,
                Headers = context.CopyHeaders(),
                Id = string.IsNullOrWhiteSpace(id) ? context.Id : id,
                SagaInfo = new SagaInfo() { Id = context.SagaInfo.Id, ParentIds = context.SagaInfo.ParentIds}
            };
        }

        public static Options CreateOptionsForParentSaga(this MessageContext context, string endpointname, string id = null)
        {
            return new Options()
            {
                EndPointName = endpointname,
                Headers = context.CopyHeaders(),
                Id = string.IsNullOrWhiteSpace(id) ? context.Id : id,
                SagaInfo = context.SagaInfo.ParentIds.Count>0 ? new SagaInfo()
                {
                    Id = context.SagaInfo.ParentIds.Last() ,
                    ParentIds = context.SagaInfo.ParentIds.Take(context.SagaInfo.ParentIds.Count - 1).ToList()
                } : new SagaInfo() { Id = context.SagaInfo.Id }
            };
        }
    }
}