using Jal.Router.Interface;
using System;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public abstract class AbstractContext
    {
        public readonly ILogger _logger;

        public Channel Channel { get; protected set; }

        public IChannelCreator ChannelCreator { get; protected set; }

        public IChannelDeleter ChannelDeleter { get; protected set; }

        public IChannelStatisticProvider ChannelProvider { get; protected set; }

        public IMessageAdapter MessageAdapter { get; protected set; }

        public IMessageSerializer MessageSerializer { get; protected set; }

        public IMessageStorage MessageStorage { get; protected set; }

        public IEntityStorage EntityStorage { get; protected set; }

        public string Id { get; protected set; }

        public readonly IHasher _hasher;

        protected AbstractContext(Channel channel, IChannelCreator channelcreator, IChannelDeleter channeldeleter, IChannelStatisticProvider channelprovider, IMessageAdapter adapter,
            IMessageSerializer serializer, IMessageStorage messagestorage, IEntityStorage entitystorage, ILogger logger, IHasher hasher)
        {
            if (channelcreator == null)
            {
                throw new ArgumentNullException(nameof(channelcreator));
            }
            if (channeldeleter == null)
            {
                throw new ArgumentNullException(nameof(channelcreator));
            }
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel));
            }
            ChannelProvider = channelprovider;
            ChannelDeleter = channeldeleter;
            ChannelCreator = channelcreator;
            Channel = channel;
            MessageAdapter = adapter;
            MessageSerializer = serializer;
            MessageStorage = messagestorage;
            EntityStorage = entitystorage;
            _logger = logger;
            _hasher = hasher;
        }

        protected void Hash()
        {
            Id = _hasher.Hash($"{Channel.ConnectionString}{Channel.Path}{Channel.Subscription}{Channel.UseCreateIfNotExists}");
        }

        public async Task CreateIfNotExist()
        {
            var created = await ChannelCreator.CreateIfNotExist(Channel).ConfigureAwait(false);

            if (created)
            {
                _logger.Log($"Created {Channel.ToString()} channel with path {Channel.FullPath}");
            }
            else
            {
                _logger.Log($"The {Channel.ToString()} channel with path {Channel.FullPath} already exists");
            }
        }

        public async Task DeleteIfExist()
        {
            var deleted = await ChannelDeleter.DeleteIfExist(Channel).ConfigureAwait(false);

            if (deleted)
            {
                _logger.Log($"Deleted {Channel.ToString()} channel with path {Channel.FullPath}");
            }
            else
            {
                _logger.Log($"The {Channel.ToString()} channel with path {Channel.FullPath} does not exist");
            }
        }

        public Task<Statistic> GetStatistic()
        {
            return ChannelProvider.GetStatistic(Channel);
        }
    }
}