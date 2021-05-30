using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Rainbow.MultiTenancy.Abstractions;
using Rainbow.MultiTenancy.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore
{
    public class RemoteTenantService : ITenantService
    {
        private readonly IOptionsSnapshot<RemoteTenantOptions> options;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IHttpClientFactory httpClientFactory;

        public RemoteTenantService(IOptionsSnapshot<RemoteTenantOptions> options
            , IHttpContextAccessor httpContextAccessor
            , IHttpClientFactory httpClientFactory)
        {
            this.options = options;
            this.httpContextAccessor = httpContextAccessor;
            this.httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient()
        {
            var client = this.httpClientFactory.CreateClient("tenant");

            var token = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Add("Authorization", token.ToString());
            }

            return client;
        }


        public async Task<List<TenantDto>> GetAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{this.options.Value.Path.RemoveTrailingSlash()}/{TenantRoutePaths.List.RemoveLeadingSlash()}");

            var response = await this.Handle(request);

            var stream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<List<TenantDto>>(stream);
        }

        public async Task<TenantDto> GetAsync(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{this.options.Value.Path.RemoveTrailingSlash()}/{TenantRoutePaths.Id.RemoveLeadingSlash()}?id={id.ToString()}");

            var response = await this.Handle(request);

            var stream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<TenantDto>(stream);
        }

        public async Task<TenantDto> GetAsync(string name)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"{this.options.Value.Path.RemoveTrailingSlash()}/{TenantRoutePaths.Name.RemoveLeadingSlash()}?name={name}");

            request.Content = new System.Net.Http.FormUrlEncodedContent(
                new Dictionary<string, string>()
                {
                    ["name"] = name,
                });
            var response = await this.Handle(request);

            var stream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<TenantDto>(stream);
        }

        private async Task<HttpResponseMessage> Handle(HttpRequestMessage message)
        {
            var client = this.CreateClient();
            var respones = await client.SendAsync(message);

            switch (respones.StatusCode)
            {
                case System.Net.HttpStatusCode.Unauthorized:
                    await Handle401(respones);
                    break;
                case System.Net.HttpStatusCode.BadRequest:
                    await Handle400(respones);
                    break;
                case System.Net.HttpStatusCode.InternalServerError:
                    await Handle500(respones);
                    break;
            }

            return respones;
        }


        private async Task Handle401(HttpResponseMessage message)
        {
            throw new Exception($"未授权 {message.StatusCode}");
        }


        private async Task Handle400(HttpResponseMessage message)
        {
            var json = await message.Content.ReadAsStringAsync();
            var error = System.Text.Json.JsonSerializer.Deserialize<ErrorContent>(json);

            throw new Exception($"参数错误 {message.StatusCode} {error.Error}");
        }

        private async Task Handle500(HttpResponseMessage message)
        {
            throw new Exception($"服务器错误 {message.StatusCode}");
        }
    }
}
