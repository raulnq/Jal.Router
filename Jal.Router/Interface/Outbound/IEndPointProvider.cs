using System;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IEndPointProvider
    {
        EndPoint Provide(Options options, object content);
    }
}