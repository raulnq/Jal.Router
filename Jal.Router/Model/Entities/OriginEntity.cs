namespace Jal.Router.Model
{
    public class OriginEntity
    {
        public string From { get; private set; }//From

        public string Key { get; private set; }//Origin

        private OriginEntity()
        {

        }

        public OriginEntity(string from, string key)
        {
            From = from;
            Key = key;
        }
    }
}
