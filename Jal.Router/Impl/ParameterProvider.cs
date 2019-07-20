using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class ParameterProvider : IParameterProvider
    {
        private IConfiguration _configuration;
        public ParameterProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T Get<T>() where T: class
        {
            if(_configuration.Parameters.ContainsKey(typeof(T).FullName))
            {
                return _configuration.Parameters[typeof(T).FullName] as T;
            }
            else
            {
                return default(T);
            }
        }
    }
}