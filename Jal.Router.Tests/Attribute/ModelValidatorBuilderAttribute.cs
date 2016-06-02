using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.NUnit2;

namespace Jal.Router.Tests.Attribute
{
    public class ModelValidatorBuilderAttribute : AutoDataAttribute
    {
        public ModelValidatorBuilderAttribute(bool valid = true, bool empty = false)
            : base(new Fixture().Customize(new AutoMoqCustomization()))
        {
           
            //var validator = Fixture.Create<Mock<IValidator<Customer>>>();

            ////validator.As<ITransientValidator>();

            //if (!valid)
            //{
            //    var message = Fixture.Create<string>();

            //    var failure = new ValidationFailure(message);

            //    validator.Setup(x => x.Validate(It.IsAny<Customer>())).Returns(new ValidationResult(new List<ValidationFailure>() { failure }));

            //    validator.Setup(x => x.Validate(It.IsAny<Customer>(), It.IsAny<string>(), It.Is<object>(o => true))).Returns(new ValidationResult(new List<ValidationFailure>() { failure }));

            //    validator.Setup(x => x.Validate(It.IsAny<Customer>(), It.IsAny<string>())).Returns(new ValidationResult(new List<ValidationFailure>() { failure }));

            //    validator.Setup(x => x.Validate(It.IsAny<Customer>(), It.Is<object>(o => true))).Returns(new ValidationResult(new List<ValidationFailure>() { failure }));
            //}
            //else
            //{
            //    validator.Setup(x => x.Validate(It.IsAny<Customer>())).Returns(new ValidationResult());

            //    validator.Setup(x => x.Validate(It.IsAny<Customer>(), It.IsAny<string>(), It.Is<object>(o => true))).Returns(new ValidationResult());

            //    validator.Setup(x => x.Validate(It.IsAny<Customer>(), It.IsAny<string>())).Returns(new ValidationResult());

            //    validator.Setup(x => x.Validate(It.IsAny<Customer>(), It.Is<object>(o => true))).Returns(new ValidationResult());
            //}


            //var factory = Fixture.Freeze<Mock<IObjectFactory>>();

            //if (!empty)
            //{
            //    factory.Setup(x => x.Create<Customer, IValidator<Customer>>(It.IsAny<Customer>())).Returns(new[] { validator.Object });

            //    factory.Setup(x => x.Create<Customer, IValidator<Customer>>(It.IsAny<Customer>(), It.IsAny<string>())).Returns(new[] { validator.Object });
            //}
            //else
            //{
            //    factory.Setup(x => x.Create<Customer, IValidator<Customer>>(It.IsAny<Customer>())).Returns(default(IValidator<Customer>[]));

            //    factory.Setup(x => x.Create<Customer, IValidator<Customer>>(It.IsAny<Customer>(), It.IsAny<string>())).Returns(default(IValidator<Customer>[]));
            //}


        }
    }
}
