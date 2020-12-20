using ChatJS.Data;
using ChatJS.WebServer;
using ChatJS.WebServer.Hubs;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ChatJS.WebServer
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddRazorPages();
            services.AddControllers();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "../ChatJS.WebApp/build";
            });

            ConfigureDatabaseServices(services);
        }

        public void ConfigureDatabaseServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString:
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AuthorizationDbContext>(options =>
                options.UseSqlServer(connectionString:
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options =>
                options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<AuthorizationDbContext>()
                    .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddApiAuthorization<IdentityUser,
                AuthorizationDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AuthorizationDbContext authDbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSpaStaticFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("chat");
            });

            app.UseSpa(spaBuilder =>
            {
                spaBuilder.Options.SourcePath = "../ChatJS.WebApp";
                if (env.IsDevelopment())
                {
                    spaBuilder.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            authDbContext.Database.Migrate();
        }
    }
}
