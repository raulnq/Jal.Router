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

        public int Timeout { get; private set; }

        public IList<Route> Routes { get; private set; }

        public IList<Route> InitialRoutes { get; private set; }

        public IList<Route> FinalRoutes { get; private set; }

        public SagaEntity ToEntity()
        {
            return new SagaEntity(Name, DataType, Timeout);
        }

        public void UpdateTimeout(int timeout)
        {
            Timeout = timeout;
        }
    }
}
