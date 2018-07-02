namespace Jal.Router.Interface
{
    public interface IMessageStorage
    {
        string Read(string id);
        void Write(string id, string content);
    }
}