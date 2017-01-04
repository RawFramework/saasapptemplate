using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApplication.Data;
using WebApplication.Models;
using WebApplication.Services;
using Microsoft.AspNetCore.Http;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            //>>>>>>>>>>>>>>>>>>>>>      ScioSaaS Start
             
            /*
             /*install/update the following packages if needed, the default template uses old versions
             * install-package Microsoft.AspNetCore.Http
             */
            //setup ScioSaaS Services
            ScioSaaSPlatform.SaaSApp.SaaSServices.SetupSaaSServices(services);
            
            //>>>>>>>>>>>>>>>>>>>>>      ScioSaaS END

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            //>>>>>>>>>>>>>>>>>>>>>      ScioSaaS Start 

            /* install/update the following packages if needed, the default template uses old versions
             * install-package Microsoft.AspNetCore.Http.Abstractions
             * install-package Microsoft.AspNetCore.DataProtection.Extensions
            */
            //Configure the SaaS application environment
            ScioSaaSPlatform.SaaSApp.SaaSEnvironment.SaaSEnvironmentSetup(app, env);

            //add a few fake users for testing, this users are only stored in memory and are only for testing purposes
            if (env.IsDevelopment())
            {
                //add sample users
                var users = new ScioSaaSPlatform.Security.AppSaaSUser.SaaSUserHelper();
                users.AddUserForTesting(new ScioSaaSPlatform.Models.SaaSUser { Id = Guid.NewGuid(), UserName = "pramirez", FirstName = "Pedro" });
                users.AddUserForTesting(new ScioSaaSPlatform.Models.SaaSUser { Id = Guid.NewGuid(), UserName = "jsmith", FirstName = "John" });
                users.AddUserForTesting(new ScioSaaSPlatform.Models.SaaSUser { Id = Guid.NewGuid(), UserName = "sdoe", FirstName = "Sarah" });

                users.AddComponentToUser("pramirez", "admin");
                
            }

            //Since the ScioSaaS platform uses custom authentication we must delete or comment that line
            //app.UseIdentity();

            //>>>>>>>>>>>>>>>>>>>>>      ScioSaaS END

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
