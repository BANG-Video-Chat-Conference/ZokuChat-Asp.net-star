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
using ZokuChat.Services;

namespace ZokuChat
{
    public class Startup
    {
		private bool _isDevEnvironment;

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
				_isDevEnvironment = true;
				builder.AddUserSecrets<Startup>();
			}

			Configuration = builder.Build();
		}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Context>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("ZokuDatabase")));

			services.AddIdentity<User, IdentityRole>()
				.AddEntityFrameworkStores<Context>()
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
				if (!_isDevEnvironment)
				{
					options.SignIn.RequireConfirmedEmail = true;
				}
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
					options.Conventions.AuthorizeFolder("/Account/Manage");
					options.Conventions.AuthorizePage("/Account/Logout");
				})
				.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// Configure service that sends email
			services.AddSingleton<IEmailSender, EmailSender>();
			services.Configure<AuthMessageSenderOptions>(Configuration);

			// Add injectable services
			services.AddTransient<IEmailService, EmailService>();
			services.AddTransient<IResolveUserService, ResolveUserService>();
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<IBlockedUserService, BlockedUserService>();
			services.AddTransient<IContactService, ContactService>();
			services.AddTransient<IContactRequestService, ContactRequestService>();
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
