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
        public string Name { get; private set; }

        public Type DataType { get; private set; }

        public int Timeout { get; set; }

        public List<Route> Routes { get; private set; }

        public List<Route> InitialRoutes { get; set; }

        public List<Route> FinalRoutes { get; set; }

        public SagaEntity ToEntity()
        {
            return new SagaEntity(Name, DataType, Timeout);
        }
    }
}
