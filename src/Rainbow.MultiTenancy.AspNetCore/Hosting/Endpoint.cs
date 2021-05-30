using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.AspNetCore.Hosting
{
    public class Endpoint
    {
        public Endpoint(string path, Type handlerType)
        {
            Path = path;
            Handler = handlerType;
        }


        public PathString Path { get; set; }

        public Type Handler { get; set; }
    }
}
