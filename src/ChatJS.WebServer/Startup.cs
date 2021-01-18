using System.Text.Json;
using System.Text.Json.Serialization;

using ChatJS.Data;
using ChatJS.Data.Builders;
using ChatJS.Data.Caching;
using ChatJS.Data.Rules;
using ChatJS.Data.Services;

using ChatJS.Domain.Chatrooms;
using ChatJS.Domain.Chatrooms.Commands;
using ChatJS.Domain.Chatrooms.Validators;
using ChatJS.Domain.Memberships;
using ChatJS.Domain.Messages;
using ChatJS.Domain.Messages.Commands;
using ChatJS.Domain.Messages.Validators;
using ChatJS.Domain.Posts;
using ChatJS.Domain.Users;
using ChatJS.Domain.Users.Commands;
using ChatJS.Domain.Users.Validators;

using ChatJS.Models.Chatrooms;
using ChatJS.Models.Memberships;
using ChatJS.Models.Messages;
using ChatJS.Models.Posts;
using ChatJS.Models.Users;

using ChatJS.WebServer.Configurations;
using ChatJS.WebServer.Hubs;
using ChatJS.WebServer.Services;

using FluentValidation;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

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
            services.AddSingleton(typeof(IHubConnectionMapper<>), typeof(HubConnectionMapper<>));
            services.AddSingleton(typeof(IHubSubscriptionMapper<>), typeof(HubSubscriptionMapper<>));

            services.AddRazorPages();
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ChatJS.WebApp/build";
            });

            ConfigureWebServices(services);
            ConfigureModelServices(services);
            ConfigureDomainServices(services);
            ConfigureDatabaseServices(services);
        }

        public void ConfigureDomainServices(IServiceCollection services)
        {
            services.AddScoped<IChatroomRules, ChatroomRules>();
            services.AddScoped<IChatroomService, ChatroomService>();
            services.AddScoped<IValidator<CreateChatroom>, CreateChatroomValidator>();
            services.AddScoped<IValidator<UpdateChatroom>, UpdateChatroomValidator>();

            services.AddScoped<IUserRules, UserRules>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IValidator<CreateUser>, CreateUserValidator>();
            services.AddScoped<IValidator<UpdateUser>, UpdateUserValidator>();

            services.AddScoped<IMembershipRules, MembershipRules>();
            services.AddScoped<IMembershipService, MembershipService>();

            services.AddScoped<IPostRules, PostRules>();
            services.AddScoped<IPostService, PostService>();

            services.AddScoped<IMessageRules, MessageRules>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IValidator<CreateMessage>, CreateMessageValidator>();
            services.AddScoped<IValidator<UpdateMessage>, UpdateMessageValidator>();
        }

        public void ConfigureModelServices(IServiceCollection services)
        {
            services.AddScoped<IChatroomModelBuilder, ChatroomModelBuilder>();
            services.AddScoped<IMembershipModelBuilder, MembershipModelBuilder>();
            services.AddScoped<IMessageModelBuilder, MessageModelBuilder>();
            services.AddScoped<IPostModelBuilder, PostModelBuilder>();
            services.AddScoped<IUserModelBuilder, UserModelBuilder>();
        }

        public void ConfigureWebServices(IServiceCollection services)
        {
            services.AddScoped<IContextService, ContextService>();
            services.AddScoped<IIntegrityService, IntegrityService>();
            services.AddScoped<INotificationService, NotificationService>();
        }

        public void ConfigureDatabaseServices(IServiceCollection services)
        {
            services.AddScoped<ICacheManager, CacheManager>();

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

            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>,
                ConfigureJwtBearerOptions>());
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

            var forwardHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                RequireHeaderSymmetry = false
            };

            forwardHeaderOptions.KnownNetworks.Clear();
            forwardHeaderOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(forwardHeaderOptions);

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("chat");
            });

            app.UseSpa(spaBuilder =>
            {
                if (env.IsDevelopment())
                {
                    spaBuilder.Options.SourcePath = "../ChatJS.WebApp";
                    spaBuilder.UseReactDevelopmentServer(npmScript: "start");
                }
                else
                {
                    spaBuilder.Options.SourcePath = "ClientApp";
                }
            });

            appContext.Database.Migrate();
            authContext.Database.Migrate();
        }
    }
}
