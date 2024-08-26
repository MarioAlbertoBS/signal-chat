namespace Chat.Tests;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;
using Chat.Data;
using Microsoft.EntityFrameworkCore;
using Chat.Models;
using Microsoft.AspNetCore.Identity;
using Chat.Services;

public class TestApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        ServiceCollection services = new ServiceCollection();
        services.AddDbContext<ChatContext>(options => options.UseInMemoryDatabase("TestDB"));
        
        // Add Identity
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ChatContext>()
            .AddDefaultTokenProviders();
        
        ServiceProvider serviceProvider = services.BuildServiceProvider();

        builder.UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureAppConfiguration((context, config) => {})
            .ConfigureServices(services => services.AddScoped(ProviderAliasAttribute => serviceProvider.GetRequiredService<ChatContext>()));

        using (IServiceScope scope = serviceProvider.CreateScope()) {
            ChatContext context = scope.ServiceProvider.GetRequiredService<ChatContext>();
            context.Database.EnsureCreated();
        }
    }
}