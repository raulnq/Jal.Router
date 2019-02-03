using System;
using System.Collections.Generic;

namespace Jal.Router.Model.Inbound
{
    public class ListenerMetadata
    {
        public Channel Channel { get; }

        public Group Group { get; set; }

        public Func<object[]> CreateListenerMethod { get; set; }

        public Action<object[]> DestroyListenerMethod { get; set; }

        public Action<object[]> ListenMethod { get; set; }

        public List<Route> Routes { get;  }

        public object[] Listener { get; set; }

        public ListenerMetadata(Channel channel)
        {
            Channel = channel;
            Routes = new List<Route>();
        }
    }
}