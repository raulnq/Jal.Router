namespace Jal.Router.Interface
{
    public interface IRuntimeListenerContextLoader
    {
        void AddPointToPointChannel<TContent, THandler, TConcreteConsumer>(string name, string connectionstring, string path);

        void AddPublishSubscribeChannel<TContent, THandler, TConcreteConsumer>(string name, string connectionstring, string path, string subscription);
    }
}