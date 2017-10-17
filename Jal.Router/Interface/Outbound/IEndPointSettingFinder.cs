using Jal.Router.Model;

namespace Jal.Router.Interface.Outbound
{
    public interface IEndPointSettingFinder<in TContent>
    {
        EndPointSetting Find(TContent content);
    }
}