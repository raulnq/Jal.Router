﻿using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class EndPointBuilder : IToEndPointBuilder, IOnEndPointOptionBuilder, INameEndPointBuilder, IToChannelBuilder, IToReplyChannelBuilder
    {
        private readonly EndPoint _endpoint;

        public EndPointBuilder(EndPoint endpoint)
        {
            _endpoint = endpoint;
        }

        public IToEndPointBuilder ForMessage<TMessage>()
        {
            _endpoint.SetContentType(typeof (TMessage));

            return this;
        }

        public void AddPointToPointChannel(string connectionstring, string path)
        {
            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var channel = new Channel(ChannelType.PointToPoint, connectionstring, path);

            _endpoint.Channels.Add(channel);
        }

        IAndWaitReplyFromEndPointBuilder IToReplyChannelBuilder.AddPointToPointChannel(string connectionstring, string path) 
        {


            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }
            var channel = new Channel(ChannelType.PointToPoint, connectionstring, path);

            _endpoint.Channels.Add(channel);

            return new AndWaitReplyFromEndPointBuilder(channel);
        }

        public void AddPublishSubscribeChannel(string connectionstring, string path)
        {


            if (string.IsNullOrWhiteSpace(connectionstring))
            {
                throw new ArgumentNullException(nameof(connectionstring));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            var channel = new Channel(ChannelType.PublishSubscribe, connectionstring, path);

            _endpoint.Channels.Add(channel);
        }

        public IOnEndPointOptionBuilder To(Action<IToChannelBuilder> channelbuilder)
        {
            if (channelbuilder==null)
            {
                throw new ArgumentNullException(nameof(channelbuilder));
            }

            channelbuilder(this);

            return this;
        }

        public IOnEndPointOptionBuilder UseMiddleware(Action<IOutboundMiddlewareBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OutboundMiddlewareBuilder(_endpoint);

            action(builder);

            return this;
        }

        public IOnEndPointOptionBuilder To<TReply>(Action<IToReplyChannelBuilder> channelbuilder)
        {
            if (channelbuilder == null)
            {
                throw new ArgumentNullException(nameof(channelbuilder));
            }

            _endpoint.SetReplyContentType(typeof(TReply));

            channelbuilder(this);

            return this;
        }

        public IOnEndPointOptionBuilder OnError(Action<IOnEndPointErrorBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnEndPointErrorBuilder(_endpoint);

            action(builder);

            return this;
        }

        public IOnEndPointOptionBuilder OnEntry(Action<IOnEndPointEntryBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnEndPointEntryBuilder(_endpoint);

            action(builder);

            return this;
        }

        public IOnEndPointOptionBuilder OnExit(Action<IOnEndPointExitBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnEndPointExitBuilder(_endpoint);

            action(builder);

            return this;
        }

        public void With(Action<IOnEndPointWithBuilder> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var builder = new OnEndPointWithBuilder(_endpoint);

            action(builder);
        }
    }
}