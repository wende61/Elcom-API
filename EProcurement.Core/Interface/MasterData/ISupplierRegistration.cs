﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EProcurement.Core.Interface.MasterData
{
    public interface ISupplierRegistration <Request ,Response>
    {
        Task<Response> RegisterAsync(Request request);
    }
}
