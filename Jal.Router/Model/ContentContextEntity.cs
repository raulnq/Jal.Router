using System;

namespace Jal.Router.Model
{
    public class ContentContextEntity
    {
        public Type Type { get; }

        public string Data { get; }

        public string Id { get; set; }

        public ContentContextEntity()
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
