using System;
using System.IO;
using System.Security.Cryptography;
using DevExpress.AspNetCore;
using HRMS.ConfigureServices;
using HRMS.Data;
using HRMS.Hubs;
using HRMS.Models;
using HRMS.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace HRMS
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
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administrator/AccessDenied");
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<HRDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("HRDbConnection")));

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 4;
                //options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                options.SignIn.RequireConfirmedEmail = true;
                options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            })
            .AddEntityFrameworkStores<HRDbContext>()
            .AddDefaultTokenProviders()
            .AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation");

            // REGISTER ExtractEMService
            ExtractEmService.ExtractEmRegisterService(services);

            services.AddDataProtection()
                .UseCustomCryptographicAlgorithms(new ManagedAuthenticatedEncryptorConfiguration()
                {
                    // A type that subclasses SymmetricAlgorithm
                    EncryptionAlgorithmType = typeof(Aes),

                    // Specified in bits [>=128]
                    EncryptionAlgorithmKeySize = 128,

                    // A type that subclasses KeyedHashAlgorithm
                    ValidationAlgorithmType = typeof(HMACSHA256)
                });

            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDevExpressControls();
            services.AddDistributedMemoryCache();
            services.AddSession();
            // HANDLE CIRCULAR LOOP
            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                //options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
            }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();

            ExtractEMPolicyService.GetExtractEmPolicyService(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");

                //app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "node_modules")),
                RequestPath = "/node_modules",

            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "RequestFiles")),
                RequestPath = "/StaticFiles"
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();
            app.UseDevExpressControls();

            app.UseRequestLocalization(app.ApplicationServices
                .GetRequiredService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseSignalR(route =>
            {
                route.MapHub<ChatHub>("/chatHub");
            });
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}");
            });
        }
    }
}
