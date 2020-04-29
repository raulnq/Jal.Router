using Jal.Router.Interface;
using Jal.Router.Model;
using System.IO;
using System.Text;

namespace Jal.Router.Impl
{
    public class FileSystemTransport : IFileSystemTransport
    {
        private IHasher _hasher;

        private IComponentFactoryFacade _factory;

        public FileSystemTransport(IHasher hasher, IComponentFactoryFacade factory)
        {
            _hasher = hasher;
            _factory = factory;
        }

        public string CreatePointToPointChannelPath(FileSystemParameter parameter, string connectionstring, string path)
        {
            var hash = _hasher.Hash(connectionstring);

            return Path.Combine(parameter.Path, hash, "queues", path);
        }

        public string CreatePublishSubscribeChannelPath(FileSystemParameter parameter, string connectionstring, string path)
        {
            var hash = _hasher.Hash(connectionstring);

            return Path.Combine(parameter.Path, hash, "topics",  path);
        }

        public string CreateSubscriptionToPublishSubscribeChannelPath(FileSystemParameter parameter, string connectionstring, string path, string subscription)
        {
            var hash = _hasher.Hash(connectionstring);

            return Path.Combine(parameter.Path, hash, "topics", path, subscription);
        }

        public void CreateFile(string path, string file, Message message, IMessageSerializer serializer)
        {
            var fullpath = Path.Combine(path, file);

            var content = serializer.Serialize(message);

            using (var fs = new FileStream(fullpath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
            {
                sw.Write(content);
            }
        }

        public Message ReadFile(string path, IMessageSerializer serializer)
        {
            var file = string.Empty;

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var sr = new StreamReader(fs, Encoding.Default))
            {
                file = sr.ReadToEnd();
            }

            return serializer.Deserialize<Message>(file);
        }

        public bool CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);

                return true;
            }

            return false;
        }

        public bool DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);

                return true;
            }

            return false;
        }

        public void DeleteFile(string path)
        {
            File.Delete(path);
        }

        public string[] GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }
    }
}