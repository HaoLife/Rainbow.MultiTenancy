using IdentityServer4.Configuration;
using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace Rainbow.MultiTenancy.IdentityServer4
{

    public class TenantEndpointRouter : IEndpointRouter
    {
        private readonly IEnumerable<global::IdentityServer4.Hosting.Endpoint> _endpoints;
        private readonly IdentityServerOptions _options;
        private readonly ILogger _logger;
        private readonly IServiceProvider provider;

        public TenantEndpointRouter(IEnumerable<global::IdentityServer4.Hosting.Endpoint> endpoints, IdentityServerOptions options, ILogger<TenantEndpointRouter> logger, IServiceProvider provider)
        {
            _endpoints = endpoints;
            _options = options;
            _logger = logger;
            this.provider = provider;
        }

        public IEndpointHandler Find(HttpContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            foreach (var endpoint in _endpoints)
            {
                var path = endpoint.Path;
                if (context.Request.Path.Equals(path, StringComparison.OrdinalIgnoreCase))
                {
                    var endpointName = endpoint.Name;
                    _logger.LogDebug("Request path {path} matched to endpoint type {endpoint}", context.Request.Path, endpointName);

                    return GetEndpointHandler(endpoint, context);
                }
            }

            _logger.LogTrace("No endpoint entry found for request path: {path}", context.Request.Path);

            return null;
        }

        private IEndpointHandler GetEndpointHandler(global::IdentityServer4.Hosting.Endpoint endpoint, HttpContext context)
        {
            if (_options.Endpoints.IsEndpointEnabled(endpoint))
            {
                if (context.RequestServices.GetService(endpoint.Handler) is IEndpointHandler handler)
                {
                    _logger.LogDebug("Endpoint enabled: {endpoint}, successfully created handler: {endpointHandler}", endpoint.Name, endpoint.Handler.FullName);
                    return new EndpointHandlerProxy(handler, this.provider);
                }

                _logger.LogDebug("Endpoint enabled: {endpoint}, failed to create handler: {endpointHandler}", endpoint.Name, endpoint.Handler.FullName);
            }
            else
            {
                _logger.LogWarning("Endpoint disabled: {endpoint}", endpoint.Name);
            }

            return null;
        }
    }
}
