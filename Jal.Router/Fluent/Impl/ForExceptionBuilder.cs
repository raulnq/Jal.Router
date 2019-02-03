using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;

namespace Jal.Router.Fluent.Impl
{
    public class ForExceptionBuilder : IForExceptionBuilder
    {
        private readonly List<Type> _list;

        public ForExceptionBuilder(List<Type> list)
        {
            _list = list;
        }

        public void For<TExeption>() where TExeption : Exception
        {
            _list.Add(typeof(TExeption));
        }
    }
}