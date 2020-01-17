using Jal.Router.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Jal.Router.Tests
{

    [TestClass]
    public class LinearRetryPolicyTests
    {
        [DataTestMethod]
        [DataRow(10,12, false)]
        [DataRow(0, 1, false)]
        [DataRow(0, 0, true)]
        [DataRow(10, 5, true)]
        public void CanRetry_With_ShouldBe(int maxcount, int currentcount, bool canretry)
        {
            var sut = new LinearRetryPolicy(0, maxcount);

            sut.CanRetry(currentcount).ShouldBe(canretry); 
        }

        [DataTestMethod]
        [DataRow(10, 12)]
        [DataRow(0, 0)]
        [DataRow(5, 15)]
        public void NextRetryInterval_With_ShouldBe(int currentcount, int seconds)
        {
            var sut = new LinearRetryPolicy(seconds, 0);

            sut.NextRetryInterval(currentcount).TotalSeconds.ShouldBe(seconds);
        }
    }
}