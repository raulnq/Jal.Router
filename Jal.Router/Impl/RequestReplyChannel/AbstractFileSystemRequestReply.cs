using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public abstract class AbstractFileSystemRequestReply : AbstractChannel
    {
        protected readonly FileSystemParameter _parameter;

        protected readonly IFileSystemTransport _transport;

        private string _path;

        protected AbstractFileSystemRequestReply(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider, IFileSystemTransport transport)
            : base(factory, logger)
        {
            _parameter = provider.Get<FileSystemParameter>();

            _transport = transport;
        }

        public void Open(SenderContext sendercontext)
        {
            _path = _transport.CreatePointToPointChannelPath(_parameter, sendercontext.Channel.ConnectionString, sendercontext.Channel.Path);
        }

        public async Task<string> Send(SenderContext sendercontext, object message)
        {
            var m = message as Message;

            if (m != null)
            {
                var filename = $"{Guid.NewGuid().ToString()}.jal";

                var handledbymock = false;

                if (_parameter.Handlers.ContainsKey(sendercontext.EndPoint.Name))
                {
                    await _parameter.Handlers[sendercontext.EndPoint.Name](sendercontext.MessageSerializer, m);

                    handledbymock = true;
                }

                if (!handledbymock)
                {
                    _transport.CreateFile(_path, filename, m, sendercontext.MessageSerializer);
                }

                return m.Id;
            }

            return string.Empty;
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