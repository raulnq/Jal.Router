using Jal.Router.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Jal.Router.Model
{
    public class InMemoryParameter
    {
        public IDictionary<string, Func<IMessageSerializer, Message, Task>> Handlers { get; private set; }

        public InMemoryParameter()
        {
            Handlers = new Dictionary<string, Func<IMessageSerializer, Message, Task>>();
        }

        public void AddEndpointHandler(string endpoint, Func<IMessageSerializer, Message, Task> action)
        {
            Handlers.Add(endpoint, action);
        }
    }
}