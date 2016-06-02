using System;
using Jal.Router.Tests.Attribute;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Shouldly;

namespace Jal.Router.Tests
{
    [TestFixture]
    public class ModelValidatorTests
    {


        //[Test]
        //[ModelValidatorBuilder(true, true)]
        //public void Validate_WithoutValidators_ShouldThrowException(RequestRouter sut)
        //{
        //    Should.Throw<Exception>(() => { sut.Validate(new Customer()); });
        //}

        //[Test]
        //[ModelValidatorBuilder]
        //public void Validate_ShouldBeValid(RequestRouter sut)
        //{
        //    var result = sut.Validate(new Customer());

        //    result.IsValid.ShouldBe(true);
        //}

        //[Test]
        //[ModelValidatorBuilder(false)]
        //public void Validate_ShouldBeInValid(RequestRouter sut)
        //{
        //    var result = sut.Validate(new Customer());

        //    result.IsValid.ShouldBe(false);

        //    result.Errors.Count.ShouldBe(1);
        //}

        //[Test]
        //[ModelValidatorBuilder]
        //public void Validate_WithRuleName_ShouldBeValid(RequestRouter sut)
        //{
        //    var rulename = new Fixture().Create<string>();

        //    var result = sut.Validate(new Customer(), rulename);

        //    result.IsValid.ShouldBe(true);
        //}


        //[Test]
        //[ModelValidatorBuilder]
        //public void Validate_WithRuleNameAndRuleSet_ShouldBeValid(RequestRouter sut)
        //{
        //    var fixture = new Fixture();

        //    var rulename = fixture.Create<string>();

        //    var ruleset = fixture.Create<string>();

        //    var result = sut.Validate(new Customer(), rulename, ruleset);

        //    result.IsValid.ShouldBe(true);
        //}

        //[Test]
        //[ModelValidatorBuilder]
        //public void Validate_WithContext_ShouldBeValid(RequestRouter sut)
        //{
        //    var result = sut.Validate(new Customer(), new { });

        //    result.IsValid.ShouldBe(true);
        //}

        //[Test]
        //[ModelValidatorBuilder(false)]
        //public void Validate_WithContext_ShouldBeNotValid(RequestRouter sut)
        //{
        //    var result = sut.Validate(new Customer(), new { });

        //    result.IsValid.ShouldBe(false);

        //    result.Errors.Count.ShouldBe(1);
        //}

        //[Test]
        //[ModelValidatorBuilder]
        //public void Validate_WithRuleNameRuleSetAndContext_ShouldBeValid(RequestRouter sut)
        //{
        //    var fixture = new Fixture();

        //    var rulename = fixture.Create<string>();

        //    var ruleset = fixture.Create<string>();

        //    var result = sut.Validate(new Customer(), rulename, ruleset, new { });

        //    result.IsValid.ShouldBe(true);
        //}

        //[Test]
        //[ModelValidatorBuilder(false)]
        //public void Validate_WithRuleNameRuleSetAndContext_ShouldNotBeValid(RequestRouter sut)
        //{
        //    var fixture = new Fixture();

        //    var rulename = fixture.Create<string>();

        //    var ruleset = fixture.Create<string>();

        //    var result = sut.Validate(new Customer(), rulename, ruleset, new { });

        //    result.IsValid.ShouldBe(false);

        //    result.Errors.Count.ShouldBe(1);
        //}
    }
}

