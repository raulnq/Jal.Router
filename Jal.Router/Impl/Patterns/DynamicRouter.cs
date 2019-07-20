using System.Linq;
using System.Threading.Tasks;
using Jal.Router.Interface.Patterns;
using Jal.Router.Model;

namespace Jal.Router.Impl.Patterns
{
    public class DynamicRouter<TData> : IDynamicRouter<TData>
    {
        private readonly IDynamicRoute<TData>[] _dynamicroutes;

        public DynamicRouter(IDynamicRoute<TData>[] dynamicroutes)
        {
            _dynamicroutes = dynamicroutes;
        }

        public Task Send(MessageContext context, TData data, string id="")
        {
            var order = 0;

            var current = _dynamicroutes.FirstOrDefault(x => x.Id== id);

            if (current != null)
            {
                order = current.Order + 1;
            }

            var @continue = true;

            IDynamicRoute<TData> route = null;

            while (@continue)
            {
                var next = _dynamicroutes.FirstOrDefault(x => x.Order == order);

                if (next != null)
                {
                    if (next.IsValid(context, data))
                    {
                        @continue = false;

                        route = next;
                    }
                    else
                    {
                        order++;
                    }
                }
                else
                {
                    @continue = false;
                }
            }

            if(route!=null)
            {
                return route.Send(context, data);
            }
            else
            {
                return Task.CompletedTask;
            }
        }
    }
}