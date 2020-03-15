namespace Jal.Router.Interface
{
    public interface IListenerContextLoader
    {
        void AddPointToPointChannel<TContent>(string name, string connectionstring, string path);

        void AddPublishSubscribeChannel<TContent>(string name, string connectionstring, string path, string subscription);
    }
}