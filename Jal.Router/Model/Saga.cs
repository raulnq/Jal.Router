using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public string Name { get; set; }

        public Type DataType { get; set; }

        public List<Route> Routes { get; set; }
    }
}
