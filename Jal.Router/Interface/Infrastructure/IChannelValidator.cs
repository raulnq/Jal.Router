using Jal.Router.Model;
using System.Text;

namespace Jal.Router.Interface
{
    public interface IChannelValidator
    {
        string Validate(Channel channel, string resourcetype, string resourcename);
    }
}