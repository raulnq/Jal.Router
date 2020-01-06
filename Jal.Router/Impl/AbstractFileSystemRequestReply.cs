using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public abstract class AbstractFileSystemRequestReply : AbstractChannel
    {
        protected readonly FileSystemParameter _parameter;

        protected readonly IFileSystem _filesystem;

        private string _path;

        protected AbstractFileSystemRequestReply(IComponentFactoryGateway factory, ILogger logger, IParameterProvider provider, IFileSystem filesystem)
            : base(factory, logger)
        {
            _parameter = provider.Get<FileSystemParameter>();

            _filesystem = filesystem;
        }

        public void Open(SenderContext sendercontext)
        {
            _path = _filesystem.CreatePointToPointChannelPath(_parameter, sendercontext.Channel.ToConnectionString, sendercontext.Channel.ToPath);
        }

        public Task<string> Send(SenderContext sendercontext, object message)
        {
            var m = message as Message;

            if (m != null)
            {
                var filename = $"{Guid.NewGuid().ToString()}.jal";

                var handledbymock = false;

                foreach (var endpoint in sendercontext.Endpoints)
                {
                    if (_parameter.Mocks.ContainsKey(endpoint.Name))
                    {
                        var serializer = Factory.CreateMessageSerializer();

                        _parameter.Mocks[endpoint.Name](_filesystem, serializer, m, filename);

                        handledbymock = true;
                    }
                }

                if (!handledbymock)
                {
                    _filesystem.CreateFile(_path, filename, m);
                }

                return Task.FromResult(m.Id);
            }

            return Task.FromResult(string.Empty);
        }

        public bool IsActive(SenderContext sendercontext)
        {
            return true;
        }

        public Task Close(SenderContext sendercontext)
        {
            return Task.CompletedTask;
        }
    }
}