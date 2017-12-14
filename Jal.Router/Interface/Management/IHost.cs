namespace Jal.Router.Interface.Management
{
    public interface IHost
    {
        void RunAndBlock();

        IConfiguration Configuration { get; }
    }
}
