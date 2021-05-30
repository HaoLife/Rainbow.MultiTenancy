using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    public class TenantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
