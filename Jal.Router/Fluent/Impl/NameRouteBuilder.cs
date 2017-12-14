using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class NameRouteBuilder<THandler> : INameRouteBuilder<THandler>, IListenerRouteBuilder<THandler>
    {
        private string _tosubscription;

        private string _topath;

        private Type _connectionstringextractortype;

        private object _toconnectionstringextractor;

        private readonly List<Route> _routes;

        private readonly string _name;

        public NameRouteBuilder(List<Route> routes, string name)
        {
            _routes = routes;

            _name = name;
        }

        public IHandlerBuilder<TContent, THandler> ForMessage<TContent>()
        {
            var value = new Route<TContent, THandler>(_name)
            {
                ToPath = _topath,

                ToSubscription = _tosubscription,

                ConnectionStringExtractorType = _connectionstringextractortype,

                ToConnectionStringExtractor = _toconnectionstringextractor
            };

            var builder = new HandlerBuilder<TContent, THandler>(value);

            _routes.Add(value);

            return builder;
        }

        public INameRouteBuilder<THandler> ToListenPointToPointChannel<TExtractorConectionString>(string path, Func<IValueSettingFinder, string> connectionstringextractor) where TExtractorConectionString : IValueSettingFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }

            _topath = path;

            _toconnectionstringextractor = connectionstringextractor;

            _connectionstringextractortype = typeof (TExtractorConectionString);

            return this;
        }

        public INameRouteBuilder<THandler> ToListenPublishSubscribeChannel<TExtractorConectionString>(string path, string subscription,
            Func<IValueSettingFinder, string> connectionstringextractor) where TExtractorConectionString : IValueSettingFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            _topath = path;

            _toconnectionstringextractor = connectionstringextractor;

            _connectionstringextractortype = typeof(TExtractorConectionString);

            _tosubscription = subscription;

            return this; 
        }
    }
}