namespace Jal.Router.Interface.Management
{
    public interface IHost
    {
        void RunAndBlock();

        void Run();

        void Startup();

        void Shutdown();

        IConfiguration Configuration { get; }
    }
}
