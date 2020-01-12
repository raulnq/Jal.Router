using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IFileSystemTransport
    {
        string CreatePointToPointChannelPath(FileSystemParameter parameter, string connectionstring, string path);

        string CreatePublishSubscribeChannelPath(FileSystemParameter parameter, string connectionstring, string path);

        string CreateSubscriptionToPublishSubscribeChannelPath(FileSystemParameter parameter, string connectionstring, string path, string subscription);

        bool CreateDirectory(string path);

        bool DeleteDirectory(string path);

        string[] GetDirectories(string path);

        void CreateFile(string path, string file, Message message);

        Message ReadFile(string path);

        void DeleteFile(string path);
    }

}