using Jal.Router.Impl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Jal.Router.Tests
{
    [TestClass]
    public class FixedRetryPolicyTests
    {
        [TestMethod]
        public void CanRetry_WithNoIntervals_ShouldBeFalse()
        {
            var sut = new FixedRetryPolicy(new int[] { });

            sut.CanRetry(1).ShouldBe(false);
        }

        [TestMethod]
        public void CanRetry_WithCurrentCountLowerThenIntervalsLength_ShouldBeTrue()
        {
            var sut = new FixedRetryPolicy(new int[] { 5, 10 });

            sut.CanRetry(0).ShouldBe(true);
        }

        [TestMethod]
        public void CanRetry_WithCurrentCountGreaterThenIntervalsLength_ShouldBeFalse()
        {
            var sut = new FixedRetryPolicy(new int[] { 5 });

            sut.CanRetry(3).ShouldBe(false);
        }

        [TestMethod]
        public void NextRetryInterval_WithCurrentCountLowerThenIntervalsLength_ShouldHaveValue()
        {
            var sut = new FixedRetryPolicy(new int[] { 5 });

            sut.NextRetryInterval(0).TotalSeconds.ShouldBe(5);
        }

        [TestMethod]
        public void NextRetryInterval_WithCurrentCountGreaterThenIntervalsLength_ShouldBeZero()
        {
            var sut = new FixedRetryPolicy(new int[] { 5 });

            sut.NextRetryInterval(4).TotalSeconds.ShouldBe(0);
        }
    }
}