namespace Jal.Router.Interface
{
    public interface IManager
    {
        void CreateSubscription(string connectionstring, string topicpath, string name, string origin);

        void CreateTopic(string connectionstring, string name);

        void CreateQueue(string connectionstring, string name);
    }
}