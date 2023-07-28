using INDO_FIN_NET.Models;
using INDO_FIN_NET.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace INDO_FIN_NET
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
            services.AddDbContext<RSSBPRODDbCotext>(options =>
                  
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection2")));
            services.AddDbContext<INDO_FinNetDbCotext>(options =>
                  options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();
            services.AddTransient<IActiveLogin, CheckLoginStatus>();
            services.AddSession();
            services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
            });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=OrganisationLogin}/{action=MainHomePage}/{id?}");
            });
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Content-Security-Policy", "frame-ancestors 'none'");

                // ** Set Content-Security-Policy header *****
                context.Response.Headers.Add("Content-Security-Policy", "default-src 'self' trusted-source.com; script-src 'self' trusted-scripts.com;");
                // ** Set Permission-Policy header *****
                context.Response.Headers.Add("Permissions-Policy", "microphone=(self), camera=(self), geolocation=(self https://rssbactopen.silsaas.co.in)");
                //*** Set Referrer-Policy header ***
                context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
                //*** Set-Content-TypeOptions ***
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                // ** Set X-Frame-Options *****
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                // **Remove default banners*****
                context.Response.Headers.Remove("Server");
                context.Response.Headers.Remove("X-Powered-By");
                await next();
            });
            //  Rotativaconfigurtion.setup
            RotativaConfiguration.Setup((Microsoft.AspNetCore.Hosting.IHostingEnvironment)env);

        }
    }
}
