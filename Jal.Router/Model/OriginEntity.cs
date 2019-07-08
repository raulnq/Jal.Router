namespace Jal.Router.Model
{
    public class OriginEntity
    {
        public string From { get; }//From

        public string Key { get;  }//Origin

        public OriginEntity()
        {

        }

        public OriginEntity(string from, string key)
        {
            From = from;
            Key = key;
        }
    }
}
