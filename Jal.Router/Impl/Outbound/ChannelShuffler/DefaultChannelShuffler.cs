﻿using Jal.Router.Interface.Outbound;
using Jal.Router.Model;

namespace Jal.Router.Impl.Outbound.ChannelShuffler
{
    public class DefaultChannelShuffler : IChannelShuffler
    {
        public Channel[] Shuffle(Channel[] channel)
        {
            return channel;
        }
    }
}