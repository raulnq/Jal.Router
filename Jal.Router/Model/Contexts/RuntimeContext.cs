using Jal.Router.Model;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class RuntimeContext
    {
        public List<ListenerContext> ListenerContexts { get; }

        public List<EndPoint> EndPoints { get; }

        public List<Saga> Sagas { get; }

        public List<Route> Routes { get; }

        public List<SenderContext> SenderContexts { get; }

        public List<AbstractContext> Contexts { get; }

        public RuntimeContext()
        {
            ListenerContexts = new List<ListenerContext>();

            SenderContexts = new List<SenderContext>();

            Contexts = new List<AbstractContext>();

            EndPoints = new List<EndPoint>();

            Sagas = new List<Saga>();

            Routes = new List<Route>();
        }
    }
}