using System;

namespace Jal.Router.Model
{
    public class EndpointEntity
    {
        public string Name { get; }

        public Type ContentType { get; }

        public EndpointEntity()
        {

        }

        public EndpointEntity(string name, Type contenttype)
        {
            Name = name;
            ContentType = contenttype;
        }
    }
}
