namespace Jal.Router.Model
{
    public class Beat
    {
        public Beat(string name, string action)
        {
            Name = name;
            Action = action;
        }
        public string Name { get; private set; }

        public string Action { get; private set; }
    }
}