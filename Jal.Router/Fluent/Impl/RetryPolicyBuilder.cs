using System;
using Jal.Router.Fluent.Interface;
using Jal.Router.Interface;
using Jal.Router.Model;

namespace Jal.Router.Fluent.Impl
{
    public class RetryPolicyBuilder<TExtractor> : INameRetryPolicyBuilder, IPolicyRetryBuilder where TExtractor : IValueSettingFinder
    {
        private readonly RetryPolicy _policy;

        public RetryPolicyBuilder(RetryPolicy policy)
        {
            _policy = policy;

            _policy.ExtractorType = typeof(TExtractor);
        }

        public IPolicyRetryBuilder ForMessage<TMessage>()
        {
            _policy.MessageType = typeof(TMessage);

            return this;
        }

        public void UsePolicy(Func<IValueSettingFinder, IRetryPolicy> policyextractor)
        {
            if (policyextractor == null)
            {
                throw new ArgumentNullException(nameof(policyextractor));
            }

            _policy.PolicyExtractor = policyextractor;
        }
    }
}