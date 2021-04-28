using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rainbow.MultiTenancy.Samples.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Samples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ValuesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public IEnumerable<string> Get()
        {
            var users = this.dbContext.Set<IdentityUser>().ToList();
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => $"{index}")
            .ToArray();
        }
    }
}
