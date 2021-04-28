using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    [Serializable]
    public class TenantConfiguration
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ConnectionStrings ConnectionStrings { get; set; }

        public TenantConfiguration()
        {

        }

        public TenantConfiguration(Guid id,string name)
        {

            Id = id;
            Name = name;

            ConnectionStrings = new ConnectionStrings();
        }
    }
}
