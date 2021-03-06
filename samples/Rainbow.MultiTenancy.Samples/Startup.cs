using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rainbow.MultiTenancy.Extensions.Identity.Stores;
using Rainbow.MultiTenancy.Samples.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.MultiTenancy.Samples
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                    .UseMultiTenancy()
            );

            services.AddMulitTenancy(options =>
            {
                options
                    .AddDomainTenantResolveContributor("{tenant}.test.com")
                    .AddHttpTenantResolveContributor()
                    //.AddDefaultTenantConfiguration(Configuration.GetSection("Tenant"))
                    ;
            }).AddTenantEntityFrameworkStores<ApplicationDbContext>();


            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<TenantUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<TenantRole>()
                .AddTenantIdentityCore()
                .AddTenantEntityFrameworkStores<ApplicationDbContext>();


            services.AddRazorPages();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseMultiTenancy();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
