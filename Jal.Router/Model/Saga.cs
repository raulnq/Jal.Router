using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Saga
    {
        public Saga(string name, Type datatype)
        {
            Name = name;

            DataType = datatype;

            Routes = new List<Route>();
        }
        public string Name { get; }

        public Type DataType { get; }

        public int Timeout { get; set; }

        public List<Route> Routes { get; }

        public Route FirstRoute { get; set; }

        public Route LastRoute { get; set; }
    }
}
