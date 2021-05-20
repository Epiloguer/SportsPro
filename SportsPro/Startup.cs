using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SportsPro.Models;
using System;

namespace SportsPro
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //configure app to use session state
            //services must be called before AddControllersWithViews()
            services.AddMemoryCache();
            //changed session timeout to 5 min; see pg 325 of Murach textbook
            services.AddSession(options=>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(60 * 5);
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
            });
           

            services.AddControllersWithViews().AddNewtonsoftJson(); // add MVC services, added NewtonsoftJson library

            services.AddDbContext<SportsProContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SportsPro")));

            services.AddRouting(options => {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;
            });

            //add other services here
        }

        // Use this method to configure the HTTP request pipeline.
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

            app.UseRouting();  // mark where routing decisions are made

            //configure middleware that runs after routing decisions have been made

            app.UseAuthorization();

            //configure app to use session state
            //services must be called before UseEndpoints()

            app.UseSession();
            app.UseEndpoints(endpoints =>    // map the endpoints
            {
                //specific route - 1 required segment
                endpoints.MapControllerRoute(
                    name: "Index",
                    pattern: "{controller}/{action=Index}/{id?}");

                //least specific route - 0 required segments
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // configure other middleware here
        }
    }
}