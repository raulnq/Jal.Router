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

            Continue = new List<Route>();
        }
        public string Name { get; set; }

        public Type DataType { get; set; }

        public List<Route> Continue { get; set; }

        public Route Start { get; set; }
    }

    public class Saga<TData> : Saga
    {
        //public Func<TData, InboundMessageContext, string> DataKeyBuilder { get; set; }

        public Saga(string name) : base(name, typeof(TData))
        {
        }
    }
}
