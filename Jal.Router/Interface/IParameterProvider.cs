namespace Jal.Router.Interface
{
    public interface IParameterProvider
    {
        T Get<T>() where T : class;
    }
}