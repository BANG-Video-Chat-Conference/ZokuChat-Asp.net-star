using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZokuChat.Areas.Identity.Data;
using ZokuChat.Models;

[assembly: HostingStartup(typeof(ZokuChat.Areas.Identity.IdentityHostingStartup))]
namespace ZokuChat.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<ZokuChatContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("ZokuChatContextConnection")));

                services.AddDefaultIdentity<ZokuChatUser>()
                    .AddEntityFrameworkStores<ZokuChatContext>();
            });
        }
    }
}