using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using OPM.SFS.Data;
using OPM.SFS.Web.Infrastructure;
using OPM.SFS.Web.Infrastructure.Extensions;
using OPM.SFS.Web.SharedCode;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace OPM.SFS.Web
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
            //services.AddHostedService<BackgroundServiceWorker>();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            services.AddRazorPages().AddFluentValidation();
            services.AddWebOptimizer(pipeline =>
            {
                pipeline.AddCssBundle("/css/sfsbase.css", "css/uswds.css", "css/uswds2.css", "css/sfs.css",
                    "css/jquery.dataTables.min.css", "css/jquery.modal.css", "css/jquery-ui.css");
                pipeline.AddJavaScriptBundle("/js/sfsbasebundle.js", "js/jquery.3.6.0.min.js", "js/popper.min.js",
                    "js/sfscommon.js", "js/fontawesome-all.min.js", "js/uswds.js",
                    "lib/jquery-validation/dist/jquery.validate.min.js", "lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js",
                    "js/sfs-validator.js", "js/uswds.js", "js/jquery.dataTables.min.js", "js/jquery.modal.min.js",
                    "js/jquery.modal.min.js", "js/idleSessionTimeout.js", "js/jquery-ui.min.js");

                pipeline.AddJavaScriptBundle("/js/institutionMapBundle.js", "js/mapdata.js", "js/usmap.js",
                    "js/datamap.js", "js/mapConfig.js");

                pipeline.AddJavaScriptBundle("/js/sfsadminDashboardBundle.js", "js/staticData.js", "js/adminStudentDashboard.js");
                pipeline.AddJavaScriptBundle("/js/sfsacademiaDashboardBundle.js", "js/academiaStudentDashboard.js");
                pipeline.AddJavaScriptBundle("/js/evfBundle.js", "js/employmentVerification.js");
                pipeline.MinifyJsFiles("js/resumeDocRepo.js", "js/documentRepo.js");

            });
            //services.AddMvc(); //for API controller

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/";
                    options.LogoutPath = "/Index";
                    options.AccessDeniedPath = "/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.EventsType = typeof(SignInHandler);
                })
                .AddOpenIdConnect(options => new LoginGovAuthentication().Configure(options, Configuration));
            services.ConfigureValidationServices(Configuration);
            services.ConfigureRepositories(Configuration);
            services.ConfigureAppServices(Configuration);
            services.AddDbContextPool<ScholarshipForServiceContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddHttpContextAccessor();
            services.AddMediatR(typeof(Startup).Assembly);
            services.AddLazyCache();
            services.AddDataProtection().PersistKeysToDbContext<ScholarshipForServiceContext>();
            services.AddApplicationInsightsTelemetry(options =>
            {
                options.ConnectionString = Configuration.GetValue<string>("ApplicationInsights:ConnectionString");
            });		

		}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");
            app.UseWebOptimizer();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto
            });


            //workaround.. - https://stackoverflow.com/questions/72488243/openidconnect-redirects-to-http-instead-of-https
            app.Use((context, next) =>
            {
                context.Request.Scheme = "https";
                context.Request.Host = new HostString(Configuration["LoginGov:RequestUrl"]);
                return next();
            });

            var policyCollection = new HeaderPolicyCollection()
                .AddContentSecurityPolicy(builder =>
                {
                    builder.AddUpgradeInsecureRequests(); // upgrade-insecure-requests
                    builder.AddBlockAllMixedContent(); // block-all-mixed-content                                   

                    builder.AddFormAction()
                                        .Self()
                                        .From("https://idp.int.identitysandbox.gov/")
                                        .From("https://secure.login.gov/");

                    builder.AddObjectSrc()
                            .None();

                    builder.AddFrameAncestors()
                            .Self();

                    builder.AddScriptSrc()
                            .Self()
                            .UnsafeInline()
                            .WithNonce();

                });
            app.UseSecurityHeaders(policyCollection);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
            });

        }

        private static void ConfigureAuth(AuthenticationOptions options, IConfiguration configuration)
        {
            options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }
    }
}
