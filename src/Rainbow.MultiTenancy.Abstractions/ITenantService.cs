using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    public interface ITenantService
    {
        Task<List<TenantDto>> GetAsync();

        Task<TenantDto> GetAsync(Guid id);
        Task<TenantDto> GetAsync(string name);
    }
}
