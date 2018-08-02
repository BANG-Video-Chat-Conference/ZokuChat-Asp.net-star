using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZokuChat.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using ZokuChat.Email;
using WebPWrecover.Services;
using ZokuChat.Data;

namespace ZokuChat
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
			var builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile(
					"appsettings.json",
					optional: false,
					reloadOnChange: true)
				.AddEnvironmentVariables();

			if (env.IsDevelopment())
			{
				builder.AddUserSecrets<Startup>();
			}

			Configuration = builder.Build();
		}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ZokuChatContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("ZokuDatabase")));

			services.AddIdentity<ZokuChatUser, IdentityRole>()
				.AddEntityFrameworkStores<ZokuChatContext>()
				.AddDefaultTokenProviders();

			services.Configure<IdentityOptions>(options =>
			{
				// Password settings
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 10;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = false;
				options.Password.RequiredUniqueChars = 6;

				// Lockout settings
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
				options.Lockout.MaxFailedAccessAttempts = 10;
				options.Lockout.AllowedForNewUsers = true;

				// User settings
				options.User.RequireUniqueEmail = true;
				options.SignIn.RequireConfirmedEmail = true;
			});

			services.ConfigureApplicationCookie(options =>
			{
				// Cookie settings
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
				options.LoginPath = "/Account/Login";
				options.AccessDeniedPath = "/Account/AccessDenied";
				options.SlidingExpiration = true;
			});

			services.AddMvc()
				.AddRazorPagesOptions(options => {
					options.Conventions.AuthorizeFolder("/Chat");
					options.Conventions.AuthorizeFolder("/Identity/Account/Manage");
				})
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// Configure service that sends email
			services.AddSingleton<IEmailSender, EmailSender>();
			services.Configure<AuthMessageSenderOptions>(Configuration);
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
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
