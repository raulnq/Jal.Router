using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jal.Router.Model
{
    public class Flow<TData>
    {
        public string Name { get; set; }

        public Activity[] Routes { get; set; }

        public Action<TData> KeyMap { get; set; }
    }

    public class Activity
    {
        public string RouteName { get; set; }
        public bool First { get; set; }
    }


    public class Activity<TContent> : Activity
    {
        public Action<TContent> KeyMap { get; set; }
    }
}
