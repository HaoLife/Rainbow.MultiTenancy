﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Abstractions
{
    public interface ITenantConfigurationProvider
    {
        Task<TenantConfiguration> GetAsync(bool saveResolveResult = false);
    }
}
