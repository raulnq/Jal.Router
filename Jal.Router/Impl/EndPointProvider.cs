﻿using System;
using System.Collections.Generic;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class EndPointProvider : IEndPointProvider
    {

        private readonly IValueSettingFinderFactory _valueSettingFinderFactory;

        private readonly IEndPointSettingFinderFactory _endPointSettingFinderFactory;

        private readonly EndPoint[] _endpoints;
        public EndPointProvider(IRouterConfigurationSource[] sources, IValueSettingFinderFactory valueSettingFinderFactory, IEndPointSettingFinderFactory endPointSettingFinderFactory)
        {
            _valueSettingFinderFactory = valueSettingFinderFactory;

            _endPointSettingFinderFactory = endPointSettingFinderFactory;

            var routes = new List<EndPoint>();

            foreach (var source in sources)
            {
                routes.AddRange(source.GetEndPoints());
            }

            _endpoints = routes.ToArray();
        }

        public EndPoint[] Provide<TContent>(string name = "")
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return _endpoints.Where(x => x.MessageType == typeof(TContent)).ToArray();
            }

            return _endpoints.Where(x => x.Name==name && x.MessageType == typeof(TContent)).ToArray();
        }

        

        public EndPointSetting Provide<TContent>(EndPoint endpoint, TContent content)
        {

            if (endpoint.ExtractorType == null)
            {
                if (endpoint.ConnectionStringExtractorType!=null && endpoint.PathExtractorType!=null)
                {
                    return ValueFinder(endpoint);
                }

                return null;
            }
            else
            {
                return Finder(endpoint, content);
            }
        }

        private EndPointSetting ValueFinder(EndPoint endpoint)
        {
            var output = new EndPointSetting();

            var extractorconnectionstring = _valueSettingFinderFactory.Create(endpoint.ConnectionStringExtractorType);

            var extractorpath = _valueSettingFinderFactory.Create(endpoint.PathExtractorType);

            var toconnectionextractor = endpoint.ToConnectionStringExtractor as Func<IValueSettingFinder, string>;

            var topathextractor = endpoint.ToPathExtractor as Func<IValueSettingFinder, string>;

            if (toconnectionextractor != null && topathextractor != null)
            {
                output.ToConnectionString = toconnectionextractor(extractorconnectionstring);

                output.ToPath = topathextractor(extractorpath);
            }

            return output;
        }
        private EndPointSetting Finder<T>(EndPoint endpoint, T record)
        {
            var extractor = _endPointSettingFinderFactory.Create<T>(endpoint.ExtractorType);

            var output = extractor.Find(record);

            return output;
        }
    }
}