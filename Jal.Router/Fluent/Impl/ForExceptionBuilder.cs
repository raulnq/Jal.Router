using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;

namespace Jal.Router.Fluent.Impl
{
    public class ForExceptionBuilder : IForExceptionBuilder
    {
        private readonly IList<Type> _list;

        public ForExceptionBuilder(IList<Type> list)
        {
            _list = list;
        }

        public void For<TExeption>() where TExeption : Exception
        {
            _list.Add(typeof(TExeption));
        }

        public void For<TExceptionA, TExceptionB>()
            where TExceptionA : Exception
            where TExceptionB : Exception
        {
            _list.Add(typeof(TExceptionA));

            _list.Add(typeof(TExceptionB));
        }
    }
}