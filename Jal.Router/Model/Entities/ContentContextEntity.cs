using System;

namespace Jal.Router.Model
{
    public class ContentContextEntity
    {
        public Type Type { get; private set; }

        public string Data { get; private set; }

        public string Id { get; private set; }

        private ContentContextEntity()
        {

        }

        public ContentContextEntity(Type type, string data, string id)
        {
            Type = type;
            Data = data;
            Id = id;
        }
    }
}
