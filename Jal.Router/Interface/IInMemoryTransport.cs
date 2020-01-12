using Jal.Router.Model;
using System;

namespace Jal.Router.Interface
{
    public interface IInMemoryTransport
    {
        string CreateName(string connectionstring, string path);

        void Create(string name);

        bool Exists(string name);

        void Dispach(string name, Message message);

        void Subscribe(string name, Action<Message> action);
    }

}