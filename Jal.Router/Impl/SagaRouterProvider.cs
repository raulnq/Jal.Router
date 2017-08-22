using System;
using System.Collections.Generic;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class SagaRouterProvider : ISagaRouterProvider
    {
        private readonly Saga[] _sagas;
        public SagaRouterProvider(IRouterConfigurationSource[] sources)
        {
            var sagas = new List<Saga>();

            foreach (var source in sources)
            {
                sagas.AddRange(source.GetSagas());
            }

            _sagas = sagas.ToArray();
        }
        public Saga Provide(string saganame)
        {
            return _sagas.FirstOrDefault(x => x.Name == saganame);
        }

        public Route Provide(Saga saga, Type type, string routername)
        {
            if (string.IsNullOrEmpty(routername))
            {
                return saga.Continue.FirstOrDefault(x => x.BodyType == type);
            }
            else
            {
                return saga.Continue.FirstOrDefault(x => x.Name == routername && x.BodyType == type);
            }
        }
    }
}