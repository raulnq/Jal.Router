using System;
using System.Collections.Generic;
using Jal.Router.Fluent.Interface;
using Jal.Router.Impl;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class NameRouteBuilder<THandler> : INameRouteBuilder<THandler>, IListenerRouteBuilder<THandler>, IListenerChannelBuilder
    {
        private readonly List<Route> _routes;

        private readonly string _name;

        private readonly IList<Channel> _channels;

        private Action<IListenerChannelBuilder> _channelbuilder;

        public NameRouteBuilder(List<Route> routes, string name)
        {
            _routes = routes;

            _name = name;

            _channels = new List<Channel>();
        }

        public IHandlerBuilder<TContent, THandler> ForMessage<TContent>()
        {
            _channelbuilder?.Invoke(this);

            var value = new Route<TContent, THandler>(_name) { Channels = _channels };

            var builder = new HandlerBuilder<TContent, THandler>(value);

            _routes.Add(value);

            return builder;
        }

        public void Add<TExtractorConectionString>(string path, Func<IValueSettingFinder, string> connectionstringextractor) where TExtractorConectionString : IValueSettingFinder
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (connectionstringextractor == null)
            {
                throw new ArgumentNullException(nameof(connectionstringextractor));
            }

            _channels.Add(new Channel
            {
                ToPath = path,

                ToConnectionStringExtractor = connectionstringextractor,

                ConnectionStringExtractorType = typeof(TExtractorConectionString)
            });
        }

        public void Add(string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (connectionstring == null)
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueSettingFinder, string> extractor = x => connectionstring;

            _channels.Add(new Channel
            {
                ToPath = path,

                ToConnectionStringExtractor = extractor,

                ConnectionStringExtractorType = typeof(NullValueSettingFinder)
            });
        }

        public void Add<TExtractorConectionString>(string path, string subscription, Func<IValueSettingFinder, string> connectionstringextractor) where TExtractorConectionString : IValueSettingFinder
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

            _channels.Add(new Channel
            {
                ToPath = path,

                ToConnectionStringExtractor = connectionstringextractor,

                ConnectionStringExtractorType = typeof(TExtractorConectionString),

                ToSubscription = subscription
            });
        }

        public void Add(string path, string subscription, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (connectionstring == null)
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            Func<IValueSettingFinder, string> extractor = x => connectionstring;

            _channels.Add(new Channel
            {
                ToPath = path,

                ToConnectionStringExtractor = extractor,

                ConnectionStringExtractorType = typeof(NullValueSettingFinder),

                ToSubscription = subscription
            });
        }

        public INameRouteBuilder<THandler> ToListenChannels(Action<IListenerChannelBuilder> channelbuilder)
        {
            if (channelbuilder == null)
            {
                throw new ArgumentNullException(nameof(channelbuilder));
            }

            _channelbuilder = channelbuilder;

            return this;
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

            _channels.Add(new Channel
            {
                ToPath = path,

                ToConnectionStringExtractor = connectionstringextractor,

                ConnectionStringExtractorType = typeof(TExtractorConectionString)
            });

            return this;
        }

        public INameRouteBuilder<THandler> ToListenPointToPointChannel(string path, string connectionstring)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            Func<IValueSettingFinder, string> extractor = x => connectionstring;

            _channels.Add(new Channel
            {
                ToPath = path,

                ToConnectionStringExtractor = extractor,

                ConnectionStringExtractorType = typeof(NullValueSettingFinder)
            });

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

            _channels.Add(new Channel
            {
                ToPath = path,

                ToConnectionStringExtractor = connectionstringextractor,

                ConnectionStringExtractorType = typeof(TExtractorConectionString),

                ToSubscription = subscription
            });

            return this;
        }

        public INameRouteBuilder<THandler> ToListenPublishSubscribeChannel(string path, string subscription, string connectionstring) 
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            Func<IValueSettingFinder, string> extractor = x => connectionstring;

            _channels.Add(new Channel
            {
                ToPath = path,

                ToConnectionStringExtractor = extractor,

                ConnectionStringExtractorType = typeof(NullValueSettingFinder),

                ToSubscription = subscription
            });

            return this;
        }
    }
}