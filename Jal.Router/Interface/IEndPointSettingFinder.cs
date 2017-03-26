using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IEndPointSettingFinder<in TContent>
    {
        EndPointSetting Find(TContent content);
    }
}