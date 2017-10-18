using System;
using Jal.Router.Interface.Inbound;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnRouteOptionBuilder
    {
        IOnRouteOptionBuilder OnErrorSendFailedMessageTo(string endpointname);
        IOnRouteOptionBuilder UsingFilter(Action<IFilterBuilder> action);
        IOnRouteOptionBuilder ForwardMessageTo(string endpointname);
    }

    public interface IOnEndpointOptionBuilder
    {
        //IOnOptionBuilder UsingBodySerializer()
        //IOnOptionBuilder UsingMetadataAdapter();
        //IOnOptionBuilder UsingBodyAdapter();
        //IOnOptionBuilder UsingLogger();
        //IOnOptionBuilder UsingInterceptor();
        //IOnOptionBuilder UsingPointToPointChannel();
        //IOnOptionBuilder UsingPublishSubscribe();
    }
}