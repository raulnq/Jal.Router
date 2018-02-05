using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Origin
    {
        public string Name { get; set; }//From

        public string Key { get; set; }//Origin

        public List<string> ParentKeys { get; set; }//ParentOrigin

        public Origin()
        {
            ParentKeys = new List<string>();
        }
    }
}