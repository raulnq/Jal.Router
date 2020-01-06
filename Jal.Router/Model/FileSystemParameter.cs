using Jal.Router.Interface;
using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class FileSystemParameter
    {
        public string Path { get; set; }

        public IDictionary<string, Action<IFileSystem, IMessageSerializer, Message, string>> Mocks { get; set; }

        public FileSystemParameter()
        {
            Mocks = new Dictionary<string, Action<IFileSystem, IMessageSerializer, Message, string>>();
        }

        public void AddEndpointHandler(string endpoint, Action<IFileSystem, IMessageSerializer, Message, string> action)
        {
            Mocks.Add(endpoint, action);
        }
    }
}