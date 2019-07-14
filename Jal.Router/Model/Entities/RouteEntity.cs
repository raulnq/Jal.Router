using System;

namespace Jal.Router.Model
{
    public class RouteEntity
    {
        public string Name { get; private set; }

        public Type ContentType { get; private set; }

        private RouteEntity()
        {

        }


        public RouteEntity(string name, Type contenttype)
        {
            Name = name;
            ContentType = contenttype;
        }
    }
}
