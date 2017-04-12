using System;
using System.Collections.Generic;
using System.Linq;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Impl
{
    public class RetryPolicyProvider : IRetryPolicyProvider
    {
        public IEndPointSettingFinderFactory Factory { get; set; }

        private readonly RetryPolicy[] _policies;
        public RetryPolicyProvider(IRouterConfigurationSource[] sources)
        {
            var policies = new List<RetryPolicy>();

            foreach (var source in sources)
            {
                policies.AddRange(source.GetRetryPolicies());
            }

            _policies = policies.ToArray();
        }

        public IRetryPolicy Provide<TContent>()
        {
            var policy = _policies.First(x => x.MessageType == typeof(TContent));

            var extractor = Factory.Create(policy.ExtractorType);

            var retrypolicyextractor = policy.PolicyExtractor as Func<IValueSettingFinder, IRetryPolicy>;

            return retrypolicyextractor?.Invoke(extractor);
        }
    }
}