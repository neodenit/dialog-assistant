using System.Linq;
using BlazorServerSignalRApp.Server.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neodenit.DialogAssistant.Areas.Identity;
using Neodenit.DialogAssistant.DataAccess;
using Neodenit.DialogAssistant.DataAccess.Repositories;
using Neodenit.DialogAssistant.Services;
using Neodenit.DialogAssistant.Shared;
using Neodenit.DialogAssistant.Shared.Interfaces;

namespace Neodenit.DialogAssistant
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddScoped<DbContext, ApplicationDbContext>();
            services.AddScoped<CookiesProvider>();

            services.AddTransient(typeof(IRepository<>), typeof(EFRepository<>));
            services.AddTransient(typeof(IDialogRepository), typeof(DialogRepository));
            services.AddTransient(typeof(IUserRepository), typeof(UserRepository));

            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IDialogService, DialogService>();
            services.AddTransient<IIdentityUserService, IdentityUserService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IGPT3Service, GPT3Service>();
            services.AddTransient<ILoggingService, LoggingService>();
            services.AddTransient<IPredictionService, PredictionService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<ITokenLimitService, TokenLimitService>();
            services.AddTransient<IPricingService, PricingService>();
            services.AddTransient<ITextService, TextService>();
            services.AddTransient<IPrivacyService, PrivacyService>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddSingleton<ISettings>(provider =>
            {
                var settings = new Settings();
                Configuration.Bind(settings);
                return settings;
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            });

            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbContext dbContext)
        {
            dbContext.Database.Migrate();

            app.UseResponseCompression();

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
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
