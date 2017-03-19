namespace Jal.Router.Fluent.Interface
{
    public interface INameRouteBuilder<TConsumer>
    {
        IConcreteConsumerBuilder<TBody, TConsumer> ForMessage<TBody>();
    }
}