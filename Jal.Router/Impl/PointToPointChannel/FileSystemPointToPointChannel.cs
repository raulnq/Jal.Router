using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemPointToPointChannel : AbstractChannel, IPointToPointChannel
    {
        private readonly FileSystemParameter _parameter;

        private readonly IFileSystemTransport _transport;

        private FileSystemWatcher _watcher;

        private string _path;

        public FileSystemPointToPointChannel(IComponentFactoryFacade factory, ILogger logger, IParameterProvider provider, IFileSystemTransport transport)
        : base(factory, logger)
        {
            _parameter = provider.Get<FileSystemParameter>();

            _transport = transport;
        }

        public Task Close(ListenerContext listenercontext)
        {
            _watcher.Dispose();

            return Task.CompletedTask;
        }

        public Task Close(SenderContext sendercontext)
        {
            return Task.CompletedTask;
        }

        public bool IsActive(ListenerContext listenercontext)
        {
            return _watcher.EnableRaisingEvents;
        }

        public bool IsActive(SenderContext sendercontext)
        {
            return true;
        }

        public void Listen(ListenerContext listenercontext)
        {
            var adapter = Factory.CreateMessageAdapter();

            _watcher.Created += async (object sender, FileSystemEventArgs e) =>
            {
                if(e.FullPath.Contains(".jal"))
                {
                    Thread.Sleep(500);

                    var message = _transport.ReadFile(e.FullPath);

                    var context = adapter.ReadMetadataFromPhysicalMessage(message);

                    Logger.Log($"Message {context.Id} arrived to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath}");

                    try
                    {
                        var handlers = new List<Task>();

                        foreach (var runtimehandler in listenercontext.Routes.Select(x => x.Consumer))
                        {
                            handlers.Add(runtimehandler(message, listenercontext.Channel));
                        }

                        await Task.WhenAll(handlers.ToArray());

                        _transport.DeleteFile(e.FullPath);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"Message {context.Id} failed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath} {ex}");
                    }
                    finally
                    {
                        Logger.Log($"Message {context.Id} completed to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath}");
                    }
                }
            };

            _watcher.EnableRaisingEvents = true;
        }

        public void Open(ListenerContext listenercontext)
        {
            var path = _transport.CreatePointToPointChannelPath(_parameter, listenercontext.Channel.ConnectionString, listenercontext.Channel.Path);

            _watcher = new FileSystemWatcher(path);
        }

        public void Open(SenderContext sendercontext)
        {
            _path = _transport.CreatePointToPointChannelPath(_parameter, sendercontext.Channel.ConnectionString, sendercontext.Channel.Path);
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
                    if (_parameter.Handlers.ContainsKey(endpoint.Name))
                    {
                        var serializer = Factory.CreateMessageSerializer();

                        _parameter.Handlers[endpoint.Name](serializer, m);

                        handledbymock = true;
                    }
                }

                if(!handledbymock)
                {
                    _transport.CreateFile(_path, filename, m);
                }

                return Task.FromResult(m.Id);
            }

            return Task.FromResult(string.Empty);
        }
    }
}