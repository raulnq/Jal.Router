using System;

namespace Jal.Router.Model
{
    public class ContentContextEntity
    {
        public string Data { get; private set; }

        public string Id { get; private set; }

        private ContentContextEntity()
        {

        }

        public ContentContextEntity(string data, string id)
        {
            Data = data;
            Id = id;
        }
    }
}
