namespace Jal.Router.Interface
{
    public interface ISenderContextLoader
    {
        void AddPublishSubscribeChannel<TMessage>(string name, string connectionstring, string path);

        void AddPointToPointChannel<TMessage>(string name, string connectionstring, string path);
    }
}