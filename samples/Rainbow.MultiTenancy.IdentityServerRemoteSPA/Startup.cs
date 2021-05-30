using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rainbow.MultiTenancy.Extensions.Identity.Stores;
using Rainbow.MultiTenancy.IdentityServerRemoteSPA.Data;

namespace Rainbow.MultiTenancy.IdentityServerRemoteSPA
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
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"))
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

            services.AddIdentityServer()
                .AddTenantIdentityServerCore()
                .AddApiAuthorization<TenantUser, ApplicationDbContext>()
                .AddTenantAspNetIdentityServer<TenantUser>();

            services.AddAuthentication()
                .AddIdentityServerJwt();
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                    builder.WithOrigins("http://localhost:4200", "https://localhost:5001")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                    );
            });
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
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();
            app.UseMultiTenancy();

            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

        }
    }
}
