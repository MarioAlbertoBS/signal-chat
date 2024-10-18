using System.Text;
using Chat.Data;
using Chat.Data.Repositories;
using Chat.Models;
using Chat.Services;
using Chat.Services.Messages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<ChatContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ChatContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters{
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? ""))
    };
    options.MapInboundClaims = true;
});

builder.Services.AddSignalR();

// Add CORS for development
string allowedHosts = builder.Configuration["Cors:AllowedHosts"] ?? "";
builder.Services.AddCors(options => {
    Console.WriteLine("Adding Origins: " + allowedHosts);
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins(allowedHosts)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials()
                          .SetPreflightMaxAge(TimeSpan.FromMinutes(10))
    );
});

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<RoomRepository>();
builder.Services.AddScoped<MessageRepository>();

var app = builder.Build();

// Use CORS Policies
app.UseCors("AllowSpecificOrigin");

// Run seeder
if (args.Contains("seed")) {
    using (var scope = app.Services.CreateScope()) {
        var services = scope.ServiceProvider;

        builder.Configuration["ConnectionStrings:DefaultConnection"] = "Server=localhost,1433;Database=Chat;User=sa;Password=DevelpmentP@ssword!;TrustServerCertificate=True;";

        try {
            var context = services.GetRequiredService<ChatContext>();

            await Seeder.Seed(context);
        } catch (Exception ex) {
            Console.WriteLine($"Error Executing Seeder: {ex.Message}");
        }
    }

    return;
}

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
// }

app.UseHttpsRedirection();

app.MapControllerRoute(
    name: "default",
    pattern: "api/{controller=Home}/{action=Index}/{id?}"
)
.WithOpenApi();

app.MapHub<ChatHub>("/chatHub")
    .RequireCors("AllowSpecificOrigin");

app.Run();

public partial class Program { }