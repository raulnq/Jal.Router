using System;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class FisherYatesChannelShuffler : IChannelShuffler
    {
        public Channel[] Shuffle(Channel[] channel)
        {
            Random rnd = new Random();
            int n = channel.Length;
            while (n > 1)
            {
                int k = rnd.Next(n--);
                var temp = channel[n];
                channel[n] = channel[k];
                channel[k] = temp;
            }
            return channel;
        }
    }
}