using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Hosting
{
    public class ContentResult : IEndpointResult
    {
        private readonly object content;

        public ContentResult(object content)
        {
            this.content = content;
        }
        public async Task ExecuteAsync(HttpContext context)
        {
            context.Response.StatusCode = 200;

            var json = JsonSerializer.Serialize(content);
            context.Response.ContentType = "application/json; charset=UTF-8";
            await context.Response.WriteAsync(json);
            await context.Response.Body.FlushAsync();
        }
    }
}
