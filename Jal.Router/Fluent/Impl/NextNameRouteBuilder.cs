using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class NextNameRouteBuilder<THandler, TData> : INextNameRouteBuilder<THandler, TData> , INextListenerRouteBuilder<THandler, TData>
    {
        public List<Route> Routes { get; set; }

        private readonly Saga _saga;

        private readonly string _name;

        private string _tosubscription;

        private string _topath;

        private Type _connectionstringextractortype;

        private object _toconnectionstringextractor;

        public NextNameRouteBuilder(Saga saga, string name)
        {
            _saga = saga;
            _name = name;
        }

        public IHandlerBuilder<TContent, THandler, TData> ForMessage<TContent>()
        {
            var value = new Route<TContent, THandler>(_name)
            {
                ToPath = _topath,

                ToSubscription = _tosubscription,

                ConnectionStringExtractorType = _connectionstringextractortype,

                ToConnectionStringExtractor = _toconnectionstringextractor
            };

            var builder = new HandlerBuilder<TContent, THandler, TData>(value);

            _saga.NextRoutes.Add(value);

            return builder;
        }

        public INextNameRouteBuilder<THandler, TData> ToListenPointToPointChannel<TExtractorConectionString>(string path, Func<IValueSettingFinder, string> connectionstringextractor) where TExtractorConectionString : IValueSettingFinder
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

            _connectionstringextractortype = typeof(TExtractorConectionString);

            return this;
        }

        public INextNameRouteBuilder<THandler, TData> ToListenPublishSubscribeChannel<TExtractorConectionString>(string path, string subscription,
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