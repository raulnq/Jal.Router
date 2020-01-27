﻿using Jal.Router.Interface;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public abstract class AbstractChannelResourceManager<T, S> : IChannelResourceManager<T, S>
    {
        public virtual Task<bool> CreateIfNotExist(T channel)
        {
            return Task.FromResult(false);
        }

        public virtual Task<bool> DeleteIfExist(T channel)
        {
            return Task.FromResult(false);
        }

        public virtual Task<S> Get(T channel)
        {
            return Task.FromResult(default(S));
        }
    }
}