namespace Jal.Router.Interface
{
    public interface IPublishSubscribeChannel : IChannelSender, IChannelCreator, IChannelReader, IChannelDeleter, IChannelStatisticProvider
    {

    }
}