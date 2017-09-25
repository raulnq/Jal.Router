using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class Starter : IStarter
    {
        private readonly IRouterConfigurationSource[] _sources;

        private readonly IValueSettingFinderFactory _factory;

        public IManager Manager { get; set; }
        public Starter(IRouterConfigurationSource[] sources, IValueSettingFinderFactory factory)
        {
            _sources = sources;
            _factory = factory;
            Manager = AbstractManager.Instance;
        }
        public void Start()
        {
            foreach (var source in _sources)
            {
                var queues = source.GetQueues();

                foreach (var queue in queues)
                {
                    CreateQueue(queue);
                }
            }

            foreach (var source in _sources)
            {
                var topics = source.GetTopics();

                foreach (var topic in topics)
                {
                    CreateTopic(topic);
                }
            }

            foreach (var source in _sources)
            {
                var susbcribers = source.GetSubscriptions();

                foreach (var susbcriber in susbcribers)
                {
                    CreateSubscription(susbcriber);
                }
            }
        }

        public void CreateSubscription(Subscription subscription)
        {
            if (subscription.ConnectionStringExtractorType != null)
            {

                var extractorconnectionstring = _factory.Create(subscription.ConnectionStringExtractorType);

                var toconnectionextractor = subscription.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;

                if (toconnectionextractor != null && !string.IsNullOrWhiteSpace(subscription.TopicPath) && !string.IsNullOrWhiteSpace(subscription.Origin.Key))
                {
                    Manager.CreateSubscription(toconnectionextractor(extractorconnectionstring), subscription.TopicPath, subscription.Name, subscription.Origin.Key);
                }
            }

        }

        public void CreateQueue(Queue queue)
        {
            if (queue.ConnectionStringExtractorType != null)
            {

                var extractorconnectionstring = _factory.Create(queue.ConnectionStringExtractorType);

                var toconnectionextractor = queue.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;

                if (toconnectionextractor != null)
                {
                    Manager.CreateQueue(toconnectionextractor(extractorconnectionstring), queue.Name);
                }
            }
        }

        public void CreateTopic(Topic topic)
        {
            if (topic.ConnectionStringExtractorType != null)
            {

                var extractorconnectionstring = _factory.Create(topic.ConnectionStringExtractorType);

                var toconnectionextractor = topic.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;

                if (toconnectionextractor != null)
                {
                    Manager.CreateTopic(toconnectionextractor(extractorconnectionstring), topic.Name);
                }
            }
        }
    }
}