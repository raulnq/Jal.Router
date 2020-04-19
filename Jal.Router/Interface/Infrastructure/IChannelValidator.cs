using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IChannelValidator
    {
        string Validate(Channel channel, string resourcetype, string resourcename);
    }
}