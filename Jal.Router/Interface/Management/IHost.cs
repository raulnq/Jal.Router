namespace Jal.Router.Interface.Management
{
    public interface IHost
    {
        void RunAndBlock();

        void Run();

        IConfiguration Configuration { get; }
    }
}
