using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Origin
    {
        public string From { get; set; }//From

        public string Key { get; set; }//Origin

        public Origin()
        {

        }

        public Origin(string from, string key)
        {
            From = from;
            Key = key;
        }

        public OriginEntity ToEntity()
        {
            return new OriginEntity(From, Key);
        }

    }
}