using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Hosting
{
    public class ParameterErrorResponse : IEndpointResult
    {
        private readonly string name;

        public ParameterErrorResponse(string name)
        {
            this.name = name;
        }

        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = 400;

            var json = JsonSerializer.Serialize(new ErrorContent
            {
                Status = -1,
                Error = $"parameter {name} error",
            });
            context.Response.ContentType = "application/json; charset=UTF-8";
            await context.Response.WriteAsync(json);
            await context.Response.Body.FlushAsync();
        }
    }
}
