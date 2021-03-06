﻿using System;
using System.Threading.Tasks;
using Jal.Router.Model;

namespace Jal.Router.Interface
{
    public interface IRouter
    {
        Task Route(MessageContext context);
    }
}