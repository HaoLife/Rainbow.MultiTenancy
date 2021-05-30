using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Hosting
{
    internal class EndpointRouter : IEndpointRouter
    {
        private readonly IEnumerable<Endpoint> _endpoints;
        private readonly ILogger _logger;

        public EndpointRouter(IEnumerable<Endpoint> endpoints, ILogger<EndpointRouter> logger)
        {
            _endpoints = endpoints;
            _logger = logger;
        }

        public IEndpointHandler Find(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach (var endpoint in _endpoints)
            {
                var path = endpoint.Path;
                if (context.Request.Path.Equals(path, StringComparison.OrdinalIgnoreCase))
                {
                    var endpointName = endpoint.Handler.Name;
                    _logger.LogDebug("Request path {path} matched to endpoint type {endpoint}", context.Request.Path, endpointName);

                    return GetEndpointHandler(endpoint, context);
                }
            }

            _logger.LogTrace("No endpoint entry found for request path: {path}", context.Request.Path);

            return null;
        }

        private IEndpointHandler GetEndpointHandler(Endpoint endpoint, HttpContext context)
        {
            if (context.RequestServices.GetService(endpoint.Handler) is IEndpointHandler handler)
            {
                _logger.LogDebug("Endpoint enabled: {endpoint}, successfully created handler: {endpointHandler}", endpoint.Handler.Name, endpoint.Handler.FullName);
                return handler;
            }
            return null;
        }
    }
}
