﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.EntityFrameworkCore
{
    public class TenantConfiguration
    {
        public Guid TenantId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }


    }
}
