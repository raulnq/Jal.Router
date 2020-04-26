using System;

namespace Jal.Router.Model
{
    public class RouteEntity
    {
        public string Name { get; private set; }

        private RouteEntity()
        {

        }


        public RouteEntity(string name)
        {
            Name = name;
        }
    }
}
