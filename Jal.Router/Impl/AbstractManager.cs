using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public abstract class AbstractManager : IManager
    {
        public static IManager Instance = new NullManager();

        public void CreateSubscription(string connectionstring, string topicpath, string name, string origin)
        {
            
        }

        public void CreateTopic(string connectionstring, string name)
        {

        }

        public void CreateQueue(string connectionstring, string name)
        {

        }
    }
}