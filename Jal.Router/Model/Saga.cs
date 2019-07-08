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

            InitialRoutes = new List<Route>();

            FinalRoutes = new List<Route>();
        }
        public string Name { get; }

        public Type DataType { get; }

        public int Timeout { get; set; }

        public List<Route> Routes { get; }

        public List<Route> InitialRoutes { get; set; }

        public List<Route> FinalRoutes { get; set; }

        public SagaEntity ToEntity()
        {
            return new SagaEntity(Name, DataType);
        }
    }
}
