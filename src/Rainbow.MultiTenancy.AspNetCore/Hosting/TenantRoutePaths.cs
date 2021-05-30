using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Hosting
{
    public static class TenantRoutePaths
    {
        public const string TenantPathPrefix = "services/tenant";

        public const string List = TenantPathPrefix + "/list";
        public const string Id = TenantPathPrefix + "/id";
        public const string Name = TenantPathPrefix + "/name";

    }
}
