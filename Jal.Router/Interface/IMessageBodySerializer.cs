namespace Jal.Router.Interface
{
    public interface IMessageBodySerializer
    {
        TContent Deserialize<TContent>(string content);

        string Serialize<TContent>(TContent content);
    }
}