using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AuthorizationAndAuthentication.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using AuthorizationAndAuthentication.Enums;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAndAuthentication
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme) .AddCookie(options =>
            {
                options.LoginPath="/Login";
                options.AccessDeniedPath="/Home/AnauthorizePage";
                options.CookieName = "123cookie";
            });

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("EmployeeOnly", policy => policy.RequireAuthenticatedUser().RequireClaim("EmployeeNumber"));
                options.AddPolicy("product-view", policy => policy.RequireAuthenticatedUser().RequireClaim("product", "1","2","3"));
                options.AddPolicy("product-edit", policy => policy.RequireAuthenticatedUser().RequireClaim("product", "2","3"));
                options.AddPolicy("product-save", policy => policy.RequireAuthenticatedUser().RequireClaim("product", "3"));

            });
            
            //services.AddMvc();
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            //  app.Run(async context =>
            // {
            //     Console.WriteLine("test123");
            //     await context.Response.WriteAsync("Hello from 2nd delegate.");
            //     //await AddConsole.WriteAsync("test");
            // });
            
            //  app.Use(async (HttpContext context, Func<Task> next) =>
            // {
            //     //do work before the invoking the rest of the pipeline       
            //     await context.Response.WriteAsync("Hello from 2nd delegate.");

            //     //do work after the rest of the pipeline has run     
            // });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
