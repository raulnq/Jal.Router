using System;

namespace Jal.Router.Model
{
    public class EndpointEntity
    {
        public string Name { get; private set; }

        private EndpointEntity()
        {

        }

        public EndpointEntity(string name)
        {
            Name = name;
        }
    }
}
