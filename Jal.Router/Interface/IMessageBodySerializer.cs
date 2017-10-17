namespace Jal.Router.Interface
{
    public interface IMessageBodySerializer
    {
        TContent Deserialize<TContent>(string body);

        string Serialize<TContent>(TContent body);
    }
}