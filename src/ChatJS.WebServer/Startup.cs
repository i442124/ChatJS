using ChatJS.Data;
using ChatJS.Data.Builders.Private;
using ChatJS.Data.Rules;
using ChatJS.Data.Services;
using ChatJS.Domain.Chatlogs;
using ChatJS.Domain.Chatlogs.Commands;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Memberships.Commands;
using ChatJS.Domain.Memberships.Validators;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Messages.Commands;
using ChatJS.Domain.Messages.Validators;
using ChatJS.Domain.Users;
using ChatJS.Domain.Users.Commands;
using ChatJS.Domain.Users.Validators;
using ChatJS.Models.Chatlog;
using ChatJS.Models.Messages;
using ChatJS.WebServer;
using ChatJS.WebServer.Hubs;
using ChatJS.WebServer.Services;

using FluentValidation;

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

            ConfigureWebServices(services);
            ConfigureDomainServices(services);
            ConfigureDatabaseServices(services);
        }

        public void ConfigureWebServices(IServiceCollection services)
        {
            services.AddScoped<IContextService, ContextService>();
            services.AddScoped<IIntegrityService, IntegrityService>();
        }

        public void ConfigureDomainServices(IServiceCollection services)
        {
            services.AddScoped<IChatlogRules, ChatlogRules>();
            services.AddScoped<IChatlogService, ChatlogService>();
            services.AddScoped<IChatlogModelBuilder, ChatlogModelBuilder>();

            services.AddScoped<IUserRules, UserRules>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IValidator<CreateUser>, CreateUserValidator>();
            services.AddScoped<IValidator<UpdateUser>, UpdateUserValidator>();

            services.AddScoped<IMembershipRules, MembershipRules>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IValidator<CreateMessage>, CreateMessageValidator>();
            services.AddScoped<IValidator<UpdateMessage>, UpdateMessageValidator>();
            services.AddScoped<IMessageModelBuilder, MessageModelBuilder>();

            services.AddScoped<IMessageRules, MessageRules>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IValidator<CreateMembership>, CreateMembershipValidator>();
            services.AddScoped<IValidator<UpdateMembership>, UpdateMembershipValidator>();
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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AuthorizationDbContext authContext, ApplicationDbContext appContext)
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

            appContext.Database.Migrate();
            authContext.Database.Migrate();
        }
    }
}
