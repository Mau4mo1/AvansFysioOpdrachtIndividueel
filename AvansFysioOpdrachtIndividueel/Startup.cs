using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvansFysioOpdrachtIndividueel.Data;
using AvansFysioOpdrachtIndividueel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Core.Data.Data;
using Core.Domain.Domain;
using Core.DomainServices;

namespace AvansFysioOpdrachtIndividueel
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
            // Standard stuff
            services.AddControllersWithViews();
            // Dependency Injection
            services.AddDbContext<FysioDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddScoped<IPatientRepo, SQLPatientRepo>();
            services.AddScoped<ITherapistRepo,SQLTherapistRepo>();
            services.AddScoped<IRepo<TreatmentModel>, SQLTreatmentRepo>();
            services.AddScoped<IRepo<PersonModel>, SQLPersonRepo>();
            services.AddScoped<ITreatmentManager, SQLTreatmentManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAsyncRepo<DiagnosisModel>, RestApiDiagnosisRepo>();
            // Authorization
            services.AddDbContext<UserDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("User")));
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config =>
                {
                    config.Cookie.Name = "Login.Cookie";
                    config.LoginPath = "/Home/Authenticate";
                });
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<UserDBContext>()
                .AddDefaultTokenProviders();
            services.AddAuthorization(config =>
            {
                var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                var defaultAuthPolicy = defaultAuthBuilder
                .RequireAuthenticatedUser()
                .Build();

                config.AddPolicy("RequireFysiotherapistRole",
                    policy => policy.RequireRole("Fysiotherapist", "Patient"));
                config.AddPolicy("RequirePatientRole",
                    policy => policy.RequireRole("Patient", "Fysiotherapist"));
                config.DefaultPolicy = defaultAuthPolicy;
            });
            
            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Cookie";
                config.LoginPath = "/Home/Login";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
