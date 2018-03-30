using System;

namespace Jal.Router.Interface
{
    public interface IMessageSerializer
    {
        TContent Deserialize<TContent>(string content);

        object Deserialize(string content, Type type);

        string Serialize<TContent>(TContent content);

        string Serialize(object content);
    }
}