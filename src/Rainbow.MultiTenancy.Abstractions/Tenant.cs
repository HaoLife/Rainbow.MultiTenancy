using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    public class Tenant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConcurrencyStamp { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreatorId { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string LastUpdatorId { get; set; }

        public List<TenantConfigurationString> ConfigurationStrings { get; set; }

    }
}
