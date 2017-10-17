﻿using System;
using System.Collections.Generic;

namespace Jal.Router.Model
{
    public class Saga
    {
        public Saga(string name, Type datatype)
        {
            Name = name;

            DataType = datatype;

            NextRoutes = new List<Route>();
        }
        public string Name { get; set; }

        public Type DataType { get; set; }

        public List<Route> NextRoutes { get; set; }

        public Route StartingRoute { get; set; }
    }

    public class Saga<TData> : Saga
    {
        public Saga(string name) : base(name, typeof(TData))
        {
        }
    }
}
