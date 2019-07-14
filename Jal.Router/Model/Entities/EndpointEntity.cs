using System;

namespace Jal.Router.Model
{
    public class EndpointEntity
    {
        public string Name { get; private set; }

        public Type ContentType { get; private set; }

        private EndpointEntity()
        {

        }

        public EndpointEntity(string name, Type contenttype)
        {
            Name = name;
            ContentType = contenttype;
        }
    }
}
