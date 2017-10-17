using System;

namespace Jal.Router.Fluent.Interface
{
    public interface IOnOptionBuilder
    {
        IOnOptionBuilder OnErrorSendFailedMessageTo(string endpointname);
        IOnOptionBuilder Filter(Action<IFilterBuilder> action);
        //IOnOptionBuilder UsingMessageAdapter();
        //IOnOptionBuilder UsingLogger();
        //IOnOptionBuilder UsingInterceptor();
        //IOnOptionBuilder ForwardMessageTo()
    }
}