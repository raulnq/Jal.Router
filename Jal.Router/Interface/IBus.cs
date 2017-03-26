using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IBus
    {
        void ReplyTo<TContent>(TContent content, Context context);

        void Send<TContent>(TContent content, Context context, string id="", string name = "");

        void Send<TContent>(TContent content, Context context, EndPointSetting endpoint, string id="");
    }
}