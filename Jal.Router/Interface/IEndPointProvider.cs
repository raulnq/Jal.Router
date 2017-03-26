using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IEndPointProvider
    {
        EndPoint[] Provide<TContent>(string name = "");

        EndPointSetting Provide<TContent>(EndPoint endPoint, TContent content);
    }
}