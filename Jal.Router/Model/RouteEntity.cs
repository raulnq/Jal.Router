using System;

namespace Jal.Router.Model
{
    public class RouteEntity
    {
        public string Name { get; }

        public Type ContentType { get; }

        public RouteEntity()
        {

        }


        public RouteEntity(string name, Type contenttype)
        {
            Name = name;
            ContentType = contenttype;
        }
    }
}
