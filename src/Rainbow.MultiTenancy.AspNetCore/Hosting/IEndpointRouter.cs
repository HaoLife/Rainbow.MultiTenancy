using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Hosting
{
    public interface IEndpointRouter
    {
        IEndpointHandler Find(HttpContext context);
    }
}
