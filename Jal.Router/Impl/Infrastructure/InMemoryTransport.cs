using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.Collections.Generic;

namespace Jal.Router.Impl
{
    public class InMemoryTransport : IInMemoryTransport
    {
        public class MemoryDispacher
        {
            public event EventHandler<Message> OnDispach;

            public void Dispach(Message message)
            {
                OnDispach?.Invoke(this, message);
            }

        }

        private readonly IDictionary<string, MemoryDispacher> _dispachers = new Dictionary<string, MemoryDispacher>();

        private IHasher _hasher;

        public InMemoryTransport(IHasher hasher)
        {
            _hasher = hasher;
        }

        public string CreateName(string connectionstring, string path)
        {
            return _hasher.Hash($"{connectionstring}{path}");
        }

        public void Create(string name)
        {
            _dispachers.Add(name, new MemoryDispacher());
        }

        public bool Exists(string name)
        {
            return _dispachers.ContainsKey(name);
        }

        public void Dispach(string name, Message message)
        {
            var dispacher = _dispachers[name];

            dispacher.Dispach(message);
        }

        public void Subscribe(string name, Action<Message> action)
        {
            var dispacher = _dispachers[name];

            dispacher.OnDispach += (o,m)=> { action(m); };
        }      
    }
}