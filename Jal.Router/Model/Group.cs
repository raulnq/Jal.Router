using System;

namespace Jal.Router.Model
{
    public class Group
    {
        public Func<MessageContext, bool> Until { get; set; }

        public Channel Channel { get; set; }
        public string Name { get; set; }

        public Group(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return "by group: " + Name;
        }
    }
}