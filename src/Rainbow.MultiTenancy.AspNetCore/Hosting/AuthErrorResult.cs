using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Hosting
{
    public class AuthErrorResult : IEndpointResult
    {
        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = 401;

            context.Response.ContentType = "application/json; charset=UTF-8";
            await context.Response.Body.FlushAsync();
        }
    }
}
