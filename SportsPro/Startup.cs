using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SportsPro.Models;
using System;
using SportsPro.DataLayer.Repositories;
using JavaScriptEngineSwitcher.ChakraCore;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using React.AspNet;

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

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddReact();

            // Make sure a JS engine is registered, or you will get an error!
            services.AddJsEngineSwitcher(options =>
            options.DefaultEngineName = ChakraCoreJsEngine.EngineName)
              .AddChakraCore();

            services.AddControllersWithViews().AddNewtonsoftJson(); // add MVC services, added NewtonsoftJson library

            services.AddDbContext<SportsProContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("SportsPro")));

            services.AddRouting(options => {
                options.LowercaseUrls = true;
                options.AppendTrailingSlash = true;
            });

            //Identity service 
            services.AddIdentity<User, IdentityRole>
                (options => {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<SportsProContext>()
                .AddDefaultTokenProviders();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<ISportsProUnitOfWork, SportsProUnitOfWork>();
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

            // Initialise ReactJS.NET. Must be before static files.
            app.UseReact(config =>
            {
                // If you want to use server-side rendering of React components,
                // add all the necessary JavaScript files here. This includes
                // your components as well as all of their dependencies.
                // See http://reactjs.net/ for more information. Example:
                //config
                //    .AddScript("~/js/First.jsx")
                //    .AddScript("~/js/Second.jsx");

                // If you use an external build too (for example, Babel, Webpack,
                // Browserify or Gulp), you can improve performance by disabling
                // ReactJS.NET's version of Babel and loading the pre-transpiled
                // scripts. Example:
                //config
                //    .SetLoadBabel(false)
                //    .AddScriptWithoutTransform("~/Scripts/bundle.server.js");
            });

            app.UseStaticFiles();

            app.UseRouting();  // mark where routing decisions are made

            //configure middleware that runs after routing decisions have been made
            //Configure app to use authentication and authorization (p.s. Order Matters!)
            app.UseAuthentication();
            app.UseAuthorization();

            //configure app to use session state
            //services must be called before UseEndpoints()

            app.UseSession();
            app.UseEndpoints(endpoints =>    // map the endpoints
            {
                //specific route - 1 required segment
                endpoints.MapAreaControllerRoute(
                    name: "admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=User}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                    name: "tech",
                    areaName: "Technician",
                    pattern: "Technician/{controller=User}/{action=Index}/{id?}");
                
                endpoints.MapControllerRoute(
                    name: "Index",
                    pattern: "{controller}/{action=Index}/{id?}");

                //least specific route - 0 required segments
                
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            SportsProContext.CreateAdminUser(app.ApplicationServices).Wait();

            // configure other middleware here
        }
    }
}