using Jal.Router.Interface;

namespace Jal.Router.Impl
{
    public class Starter : IStarter
    {
        private readonly IStep[] _steps;

        public Starter(IStep[] steps)
        {
            _steps = steps;
        }

        public void Start()
        {
            foreach (var step in _steps)
            {
                step.Run();
            }
        }
    }
}