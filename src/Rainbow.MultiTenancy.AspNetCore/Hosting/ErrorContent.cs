using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Hosting
{
    public class ErrorContent
    {
        public int Status { get; set; }
        public string Error { get; set; }
    }
}
