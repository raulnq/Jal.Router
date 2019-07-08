using System;

namespace Jal.Router.Model
{
    public class ContentContext
    {
        public Type Type { get; private set; }

        public string Data { get; private set; }

        public string Id { get; private set; }

        public object Response { get; private set; }

        public ContentContext()
        {

        }

        public ContentContext(Type type)
        {
            Type = type;
        }

        public ContentContext(Type type, string data): this(type)
        {
            Data = data;
        }

        public void UpdateResponse(object response)
        {
            Response = response;
        }

        public void UpdateType(Type type)
        {
            Type = type;
        }

        public void UpdateId(string id)
        {
            Id = id;
        }

        public void CreateId()
        {
            Id = Guid.NewGuid().ToString();
        }

        public void UpdateData(string content)
        {
            Data = content;
        }

        public void CleanData()
        {
            Data = string.Empty;
        }

        public ContentContextEntity ToEntity()
        {
            return new ContentContextEntity(Type, Data, Id);
        }
    }
}