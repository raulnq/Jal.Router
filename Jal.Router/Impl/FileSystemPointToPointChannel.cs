using Jal.Router.Interface;
using Jal.Router.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Jal.Router.Impl
{
    public class FileSystemPointToPointChannel : AbstractChannel, IPointToPointChannel
    {
        private readonly FileSystemParameter _parameter;

        public FileSystemPointToPointChannel(IComponentFactoryGateway factory, ILogger logger, IParameterProvider provider)
    : base(factory, logger)
        {
            _parameter = provider.Get<FileSystemParameter>();
        }

        public FileSystemPointToPointChannel(IComponentFactoryGateway factory, ILogger logger) : base(factory, logger)
        {
        }

        private FileSystemWatcher _watcher;

        private string _path;

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

            var serializer = Factory.CreateMessageSerializer();

            _watcher.Created += async (object sender, FileSystemEventArgs e) =>
            {
                if(e.FullPath.Contains(".jal"))
                {
                    var file = File.ReadAllText(e.FullPath);

                    var fmessage = serializer.Deserialize<FileSystemMessage>(file);

                    var context = adapter.ReadMetadataFromPhysicalMessage(fmessage);

                    Logger.Log($"Message {context.Id} arrived to {listenercontext.Channel.ToString()} channel {listenercontext.Channel.FullPath}");

                    try
                    {
                        var handlers = new List<Task>();

                        foreach (var runtimehandler in listenercontext.Routes.Select(x => x.Consumer))
                        {
                            handlers.Add(runtimehandler(fmessage, listenercontext.Channel));
                        }

                        await Task.WhenAll(handlers.ToArray());

                        File.Delete(e.FullPath);
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
            var path = FileSystemChannelResource.CreatePointToPointChannelPath(_parameter, listenercontext.Channel.ToConnectionString, listenercontext.Channel.ToPath);

            _watcher = new FileSystemWatcher(path);
        }

        public void Open(SenderContext sendercontext)
        {
            _path = FileSystemChannelResource.CreatePointToPointChannelPath(_parameter, sendercontext.Channel.ToConnectionString, sendercontext.Channel.ToPath);
        }

        public Task<string> Send(SenderContext sendercontext, object message)
        {
            var fmessage = message as FileSystemMessage;

            if (fmessage != null)
            {
                var filename = $"{Guid.NewGuid().ToString()}.jal";

                var fullpath = Path.Combine(_path, filename);

                var serializer = Factory.CreateMessageSerializer();

                File.WriteAllText(fullpath, serializer.Serialize(fmessage));

                return Task.FromResult(fmessage.Id);
            }

            return Task.FromResult(string.Empty);
        }
    }
}